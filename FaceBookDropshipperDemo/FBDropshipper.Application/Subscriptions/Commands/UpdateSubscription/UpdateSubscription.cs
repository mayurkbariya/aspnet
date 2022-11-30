using System.Threading;
using System.Threading.Tasks;
using FBDropshipper.Application.Exceptions;
using FBDropshipper.Application.Extensions;
using FBDropshipper.Application.Subscriptions.Models;
using FBDropshipper.Domain.Entities;
using FBDropshipper.Persistence.Context;
using FBDropshipper.Persistence.Extension;
using FluentValidation;
using MediatR;

namespace FBDropshipper.Application.Subscriptions.Commands.UpdateSubscription
{
    public class UpdateSubscriptionRequestModel : IRequest<UpdateSubscriptionResponseModel>
    {
        public int Id { get; set; }
        public int TotalProducts { get; set; }
        public int TotalMarketPlace { get; set; }
        public int TotalTeamMembers { get; set; }
    }

    public class UpdateSubscriptionRequestModelValidator : AbstractValidator<UpdateSubscriptionRequestModel>
    {
        public UpdateSubscriptionRequestModelValidator()
        {
            RuleFor(p => p.Id).Required();
            RuleFor(p => p.TotalProducts).Required();
            RuleFor(p => p.TotalMarketPlace).Required();
            RuleFor(p => p.TotalTeamMembers).Required();
        }
    }

    public class
        UpdateSubscriptionRequestHandler : IRequestHandler<UpdateSubscriptionRequestModel,
            UpdateSubscriptionResponseModel>
    {
        private readonly ApplicationDbContext _context;

        public UpdateSubscriptionRequestHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<UpdateSubscriptionResponseModel> Handle(UpdateSubscriptionRequestModel request,
            CancellationToken cancellationToken)
        {
            var subscription = await _context.Subscriptions.GetByReadOnlyAsync(p => p.Id == request.Id, cancellationToken: cancellationToken);
            if (subscription == null)
            {
                throw new NotFoundException(nameof(subscription));
            }

            subscription.TotalProducts = request.TotalProducts;
            subscription.TotalMarketPlace = request.TotalMarketPlace;
            subscription.TotalTeamMembers = request.TotalTeamMembers;
            _context.Subscriptions.Update(subscription);
            await _context.SaveChangesAsync(cancellationToken);
            return new UpdateSubscriptionResponseModel(subscription);
        }

    }

    public class UpdateSubscriptionResponseModel : SubscriptionDto
    {
        public UpdateSubscriptionResponseModel(Subscription subscription) : base(subscription)
        {
        }
    }
}