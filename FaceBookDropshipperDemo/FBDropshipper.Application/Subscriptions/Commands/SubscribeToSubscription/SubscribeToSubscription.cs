using System;
using System.Threading;
using System.Threading.Tasks;
using FBDropshipper.Application.Exceptions;
using FBDropshipper.Application.Extensions;
using FBDropshipper.Application.Interfaces;
using FBDropshipper.Domain.Entities;
using FBDropshipper.Domain.Enum;
using FBDropshipper.Persistence.Context;
using FBDropshipper.Persistence.Extension;
using FluentValidation;
using MediatR;

namespace FBDropshipper.Application.Subscriptions.Commands.SubscribeToSubscription
{
    public class SubscribeToSubscriptionRequestModel : IRequest<SubscribeToSubscriptionResponseModel>
    {
        public int SubscriptionId { get; set; }
    }

    public class SubscribeToSubscriptionRequestModelValidator : AbstractValidator<SubscribeToSubscriptionRequestModel>
    {
        public SubscribeToSubscriptionRequestModelValidator()
        {
            RuleFor(p => p.SubscriptionId).Required();
        }
    }

    public class
        SubscribeToSubscriptionRequestHandler : IRequestHandler<SubscribeToSubscriptionRequestModel,
            SubscribeToSubscriptionResponseModel>
    {
        private readonly ApplicationDbContext _context;
        private readonly IStripeService _stripeService;
        private readonly ISessionService _sessionService;
        public SubscribeToSubscriptionRequestHandler(ApplicationDbContext context, IStripeService stripeService, ISessionService sessionService)
        {
            _context = context;
            _stripeService = stripeService;
            _sessionService = sessionService;
        }

        public async Task<SubscribeToSubscriptionResponseModel> Handle(SubscribeToSubscriptionRequestModel request,
            CancellationToken cancellationToken)
        {
            var subscription = await _context.Subscriptions.GetByAsync(p => p.Id == request.SubscriptionId, cancellationToken: cancellationToken);
            if (subscription == null)
            {
                throw new NotFoundException();
            }

            var userId = _sessionService.GetUserId();
            var userCard = await _context.UserCards.GetByAsync(p => p.UserId == userId, cancellationToken: cancellationToken);
            if (userCard == null)
            {
                throw new NotFoundException();
            }
            var currentDate = DateTime.UtcNow;

            var currentMemberShip = await 
                _context.UserSubscriptions.GetByAsync(p =>
                    p.UserId == userId && p.IsCancelled == false, cancellationToken: cancellationToken);
            if (currentMemberShip != null)
            {
                if (currentMemberShip.SubscriptionId == request.SubscriptionId)
                {
                    throw new BadRequestException("Already Subscribed to a Subscription. Cannot resubscribe to an existing subscription");
                }
                if (currentMemberShip.SubscriptionId != request.SubscriptionId)
                {
                    var newSub = await _stripeService.UpdateSubscription(currentMemberShip.StripeSubscriptionId, subscription.StripePriceId);
                    currentMemberShip.IsCancelled = true;
                    var newUserSubscription = new UserSubscription()
                    {
                        UserId = userId,
                        SubscriptionId = request.SubscriptionId,
                        Amount = subscription.Amount,
                        IsCancelled = false,
                        StripeSubscriptionId = newSub.Id,
                        StripePriceId = subscription.StripePriceId,
                        Status = newSub.Status,
                        CanCancel = true,
                        CanExpire = true,
                        SubscriptionType = ((SubscriptionType) subscription.SubscriptionType).ToString(),
                    };
                    _context.UserSubscriptions.Update(currentMemberShip);
                    await _context.UserSubscriptions.AddAsync(newUserSubscription, cancellationToken);
                    await _context.SaveChangesAsync(cancellationToken);
                    return new SubscribeToSubscriptionResponseModel();
                }
            }

            var oldMemberShip =
                await _context.UserSubscriptions.GetByAsync(p =>
                    p.UserId == userId && p.IsActive && p.IsCancelled, cancellationToken: cancellationToken);
            if (oldMemberShip != null)
            {
                if (oldMemberShip.CurrentPeriodEnd >= DateTime.UtcNow)
                {
                    throw new BadRequestException("Cannot Subscribe to any subscription until the current one ends");
                }
            }
            var sub = await _stripeService.SubscribeToSubscription(userCard.StripeToken, subscription.StripePriceId);
            var userSubscription = new UserSubscription()
            {
                UserId = userId,
                SubscriptionId = request.SubscriptionId,
                Amount = subscription.Amount,
                IsCancelled = false,
                StripeSubscriptionId = sub.Id,
                StripePriceId = subscription.StripePriceId,
                Status = sub.Status,
                CanCancel = true,
                CanExpire = true,
                SubscriptionType = ((SubscriptionType) subscription.SubscriptionType).ToString(),
            };
            await _context.UserSubscriptions.AddAsync(userSubscription, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return new SubscribeToSubscriptionResponseModel();
        }

    }

    public class SubscribeToSubscriptionResponseModel
    {

    }
}