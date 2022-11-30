using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FBDropshipper.Application.Interfaces;
using FBDropshipper.Application.UserSubscriptions.Models;
using FBDropshipper.Persistence.Context;
using FBDropshipper.Persistence.Extension;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FBDropshipper.Application.UserSubscriptions.Queries.GetUserSubscriptions
{
    public class GetUserSubscriptionsRequestModel : IRequest<GetUserSubscriptionsResponseModel>
    {

    }

    public class GetUserSubscriptionsRequestModelValidator : AbstractValidator<GetUserSubscriptionsRequestModel>
    {
        public GetUserSubscriptionsRequestModelValidator()
        {

        }
    }

    public class
        GetUserSubscriptionsRequestHandler : IRequestHandler<GetUserSubscriptionsRequestModel,
            GetUserSubscriptionsResponseModel>
    {
        private readonly ApplicationDbContext _context;
        private readonly ISessionService _sessionService;
        public GetUserSubscriptionsRequestHandler(ApplicationDbContext context, ISessionService sessionService)
        {
            _context = context;
            _sessionService = sessionService;
        }

        public async Task<GetUserSubscriptionsResponseModel> Handle(GetUserSubscriptionsRequestModel request,
            CancellationToken cancellationToken)
        {
            var userId = _sessionService.GetUserId();
            var list = await _context.UserSubscriptions.GetAll(p => p.UserId == userId)
                .Select(UserSubscriptionSelector.MerchantSelector)
                .ToListAsync(cancellationToken);
            return new GetUserSubscriptionsResponseModel()
            {
                Data = list
            };
        }

    }

    public class GetUserSubscriptionsResponseModel
    {
        public List<UserSubscriptionDto> Data { get; set; }
    }
}