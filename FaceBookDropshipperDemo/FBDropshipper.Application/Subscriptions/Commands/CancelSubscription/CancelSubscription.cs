using System;
using System.Threading;
using System.Threading.Tasks;
using FBDropshipper.Application.Exceptions;
using FBDropshipper.Application.Extensions;
using FBDropshipper.Application.Interfaces;
using FBDropshipper.Persistence.Context;
using FBDropshipper.Persistence.Extension;
using FluentValidation;
using MediatR;

namespace FBDropshipper.Application.Subscriptions.Commands.CancelSubscription
{
    public class CancelSubscriptionRequestModel : IRequest<CancelSubscriptionResponseModel>
    {
        public string UserId { get; set; }
    }

    public class CancelSubscriptionRequestModelValidator : AbstractValidator<CancelSubscriptionRequestModel>
    {
        public CancelSubscriptionRequestModelValidator()
        {
            RuleFor(p => p.UserId).Required();
        }
    }

    public class
        CancelSubscriptionRequestHandler : IRequestHandler<CancelSubscriptionRequestModel,
            CancelSubscriptionResponseModel>
    {
        private readonly ApplicationDbContext _context;
        private readonly ISessionService _sessionService;
        private readonly IStripeService _stripeService;
        public CancelSubscriptionRequestHandler(ApplicationDbContext context, IStripeService stripeService, ISessionService sessionService)
        {
            _context = context;
            _stripeService = stripeService;
            _sessionService = sessionService;
        }

        public async Task<CancelSubscriptionResponseModel> Handle(CancelSubscriptionRequestModel request,
            CancellationToken cancellationToken)
        {
            var currentMemberShip = await 
                _context.UserSubscriptions.GetByAsync(p =>
                        p.UserId == request.UserId && p.IsCancelled == false, cancellationToken: cancellationToken);
            if (currentMemberShip == null)
            {
                throw new NotFoundException();
            }
            await _stripeService.CancelSubscription(currentMemberShip.StripeSubscriptionId);
            currentMemberShip.IsCancelled = true;
            currentMemberShip.CancelledDate = DateTime.UtcNow;
            _context.UserSubscriptions.Update(currentMemberShip);
            await _context.SaveChangesAsync(cancellationToken);
            
            return new CancelSubscriptionResponseModel();
        }

    }

    public class CancelSubscriptionResponseModel
    {

    }
}