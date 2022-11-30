using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FBDropshipper.Application.Interfaces;
using FBDropshipper.Common.Extensions;
using FBDropshipper.Domain.Entities;
using FBDropshipper.Domain.Enum;
using FBDropshipper.Persistence.Context;
using FBDropshipper.Persistence.Extension;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FBDropshipper.Application.Subscriptions.Commands.ImportSubscription
{
    public class ImportSubscriptionRequestModel : IRequest<ImportSubscriptionResponseModel>
    {

    }

    public class ImportSubscriptionRequestModelValidator : AbstractValidator<ImportSubscriptionRequestModel>
    {
        public ImportSubscriptionRequestModelValidator()
        {

        }
    }

    public class
        ImportSubscriptionRequestHandler : IRequestHandler<ImportSubscriptionRequestModel,
            ImportSubscriptionResponseModel>
    {
        private readonly ApplicationDbContext _context;
        private readonly IStripeService _stripeService;
        public ImportSubscriptionRequestHandler(ApplicationDbContext context, IStripeService stripeService)
        {
            _context = context;
            _stripeService = stripeService;
        }

        public async Task<ImportSubscriptionResponseModel> Handle(ImportSubscriptionRequestModel request,
            CancellationToken cancellationToken)
        {
            var list = await _stripeService.GetAllPrices();
            list = list.Where(p => p.Product.Active).ToList();
            var subscriptions = await _context.Subscriptions.GetAllReadOnly()
                .ToListAsync(cancellationToken);
            var newSubs = new List<Subscription>();
            list.ForEach(sub =>
            {
                var s = subscriptions.FirstOrDefault(p => p.StripeProductId == sub.ProductId);
                if (s == null)
                {
                    newSubs.Add(new Subscription()
                    {
                        Title = sub.Product.Name,
                        Description = sub.Product.Description,
                        StripeProductId = sub.ProductId,
                        StripePriceId = sub.Id,
                        SubscriptionType = sub.Recurring.Interval == "year" ? SubscriptionType.year.ToInt() : SubscriptionType.month.ToInt(),
                        TotalProducts = 0,
                        TotalTeamMembers = 0,
                        TotalMarketPlace = 0,
                        Amount = (sub.UnitAmount ?? 0) / (double)100,
                    });
                }
                else
                {
                    s.Title = sub.Product.Name;
                    s.Description = sub.Product.Description;
                    s.Amount = (sub.UnitAmount ?? 0) / (double)100;
                    s.IsDeleted = !sub.Product.Active;
                }
            });
            if (subscriptions.Any())
            {
                _context.Subscriptions.UpdateRange(subscriptions);
            }
            if (newSubs.Any())
            {
                _context.Subscriptions.AddRange(newSubs);
            }
            await _context.SaveChangesAsync(cancellationToken);
            return new ImportSubscriptionResponseModel();
        }

    }

    public class ImportSubscriptionResponseModel
    {

    }
}