using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FBDropshipper.Application.Interfaces;
using FBDropshipper.Application.Subscriptions.Commands.CancelSubscription;
using FBDropshipper.Common.Extensions;
using FBDropshipper.Domain.Constant;
using FBDropshipper.Domain.Entities;
using FBDropshipper.Persistence.Context;
using FBDropshipper.Persistence.Extension;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Stripe;

namespace FBDropshipper.Application.Stripe.Commands.StripeWebHook
{
    public class StripeWebHookRequestModel : IRequest<StripeWebHookResponseModel>
    {
        public string Json { get; set; }
    }

    public class StripeWebHookRequestModelValidator : AbstractValidator<StripeWebHookRequestModel>
    {
        public StripeWebHookRequestModelValidator()
        {
        }
    }

    public class StripeWebHookRequestHandler : IRequestHandler<StripeWebHookRequestModel, StripeWebHookResponseModel>
    {
        private readonly ApplicationDbContext _context;
        private readonly IBackgroundTaskQueueService _queueService;
        public StripeWebHookRequestHandler(ApplicationDbContext context, IBackgroundTaskQueueService queueService)
        {
            _context = context;
            _queueService = queueService;
        }

        public async Task<StripeWebHookResponseModel> Handle(StripeWebHookRequestModel request,
            CancellationToken cancellationToken)
        {
            var stripeEvent = EventUtility.ParseEvent(request.Json);
            var chargeService = new ChargeService();
            Charge charge = null;
            if (stripeEvent.Data.Object is PaymentIntent paymentIntent)
            {
                var charges = await chargeService.ListAsync(new ChargeListOptions()
                {
                    PaymentIntent = paymentIntent.Id,
                    Expand = new List<string>()
                    {
                        "invoice.subscription",
                        "customer"
                    }
                }, new RequestOptions()
                {
                },cancellationToken);
                if (charges.Any())
                {
                    charge = charges.First();
                }
            }
            if (stripeEvent.Data.Object is Invoice invoice)
            {
                if (invoice.ChargeId.IsNotNullOrWhiteSpace())
                {
                    charge = await chargeService.GetAsync(invoice.ChargeId, new ChargeGetOptions()
                    {
                        Expand = new List<string>()
                        {
                            "invoice.subscription",
                            "customer"
                        }
                    }, cancellationToken: cancellationToken);
                }
            }

            if (charge != null)
            {
                var subscriptionId = charge.Invoice.SubscriptionId;
                    if (string.IsNullOrWhiteSpace(subscriptionId))
                    {
                        return new StripeWebHookResponseModel();
                    }
                    var merchantSub = await 
                        _context.UserSubscriptions.GetByAsync(p => 
                                p.IsCancelled == false &&
                                p.StripeSubscriptionId == subscriptionId,
                            p => p.Include(pr => pr.User),
                            cancellationToken);
                    if (merchantSub != null)
                    {
                        if (stripeEvent.Type == Events.InvoicePaid || stripeEvent.Type == "invoice.payment_succeeded")
                        {
                            merchantSub.IsActive = true;
                            merchantSub.CurrentPeriodEnd = charge.Invoice.Subscription.CurrentPeriodEnd;
                            merchantSub.CurrentPeriodStart = charge.Invoice.Subscription.CurrentPeriodStart;
                            var isExist = _context.AppTransactions.Any(p => p.StripePaymentId == charge.Id);
                            if (!isExist)
                            {
                                var appTransaction = new AppTransaction()
                                {
                                    Amount = (double)charge.Amount / 100,
                                    UserId = merchantSub.UserId,
                                    Status = charge.Status,
                                    StripeSubscriptionId = subscriptionId,
                                    StripePaymentId = charge.Id,
                                    Url = charge.ReceiptUrl,
                                    InvoiceDate = charge.Created,
                                    Fee = 0,
                                    FromDate = charge.Invoice.Subscription.CurrentPeriodStart,
                                    ToDate = charge.Invoice.Subscription.CurrentPeriodEnd
                                };
                                await _context.AppTransactions.AddAsync(appTransaction, cancellationToken);
                            }
                            _context.UserSubscriptions.Update(merchantSub);
                            await _context.SaveChangesAsync(cancellationToken);
                            
                        }
                        if (stripeEvent.Type == Events.PaymentIntentCanceled)
                        {
                            _queueService.QueueBackgroundWorkItem(new CancelSubscriptionRequestModel()
                            {
                                UserId = merchantSub.UserId
                            });
                        }
                        if (stripeEvent.Type == Events.InvoicePaymentFailed)
                        {
                            _queueService.QueueBackgroundWorkItem(new CancelSubscriptionRequestModel()
                            {
                                UserId = merchantSub.UserId
                            });
                        }
                    }
            }
            return new StripeWebHookResponseModel();
        }
    }

    public class StripeWebHookResponseModel
    {
    }
}