using FBDropshipper.Application.Interfaces;
using FBDropshipper.Application.UserSubscriptions.Models;
using FBDropshipper.Common.Extensions;
using FBDropshipper.Persistence.Context;
using FBDropshipper.Persistence.Extension;
using FluentValidation;
using MediatR;

namespace FBDropshipper.Application.UserSubscriptions.Queries.GetCurrentUserSubscription;

public class GetCurrentUserSubscriptionRequestModel : IRequest<GetCurrentUserSubscriptionResponseModel>
{

}

public class GetCurrentUserSubscriptionRequestModelValidator : AbstractValidator<GetCurrentUserSubscriptionRequestModel>
{
    public GetCurrentUserSubscriptionRequestModelValidator()
    {

    }
}

public class
    GetCurrentUserSubscriptionRequestHandler : IRequestHandler<GetCurrentUserSubscriptionRequestModel,
        GetCurrentUserSubscriptionResponseModel>
{
    private readonly ApplicationDbContext _context;
    private readonly ISessionService _sessionService;
    public GetCurrentUserSubscriptionRequestHandler(ApplicationDbContext context, ISessionService sessionService)
    {
        _context = context;
        _sessionService = sessionService;
    }

    public async Task<GetCurrentUserSubscriptionResponseModel> Handle(GetCurrentUserSubscriptionRequestModel request,
        CancellationToken cancellationToken)
    {
        var userId = _sessionService.GetUserId();
        var subs = await _context.UserSubscriptions.GetByWithSelectAsync(p => p.UserId == userId && p.IsActive,
            UserSubscriptionSelector.MerchantSelector, cancellationToken: cancellationToken);
        if (subs == null)
        {
            return new GetCurrentUserSubscriptionResponseModel();
        }
        return subs.CreateCopy<GetCurrentUserSubscriptionResponseModel>();
    }

}

public class GetCurrentUserSubscriptionResponseModel : UserSubscriptionDto 
{
    
}