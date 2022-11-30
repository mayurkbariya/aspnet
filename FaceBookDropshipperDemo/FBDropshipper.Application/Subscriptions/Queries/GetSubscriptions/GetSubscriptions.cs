using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using FBDropshipper.Application.Shared;
using FBDropshipper.Application.Subscriptions.Models;
using FBDropshipper.Common.Extensions;
using FBDropshipper.Domain.Entities;
using FBDropshipper.Persistence.Context;
using FBDropshipper.Persistence.Extension;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FBDropshipper.Application.Subscriptions.Queries.GetSubscriptions
{
    public class GetSubscriptionsRequestModel : GetPagedRequest<GetSubscriptionsResponseModel>
    {

    }

    public class GetSubscriptionsRequestModelValidator : PageRequestValidator<GetSubscriptionsRequestModel>
    {
        public GetSubscriptionsRequestModelValidator()
        {

        }
    }

    public class
        GetSubscriptionsRequestHandler : IRequestHandler<GetSubscriptionsRequestModel, GetSubscriptionsResponseModel>
    {
        private readonly ApplicationDbContext _context;

        public GetSubscriptionsRequestHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<GetSubscriptionsResponseModel> Handle(GetSubscriptionsRequestModel request,
            CancellationToken cancellationToken)
        {
            Expression<Func<Subscription, bool>> query = p => true;
            if (request.Search.IsNotNullOrWhiteSpace())
            {
                query = p => 
                    p.Description.ToLower().Contains(request.Search) ||
                    p.Title.ToLower().Contains(request.Search);
            }

            var list = await _context.Subscriptions.GetManyReadOnly(query, request)
                .Select(SubscriptionSelector.Selector)
                .ToListAsync(cancellationToken);
            var count = await _context.Subscriptions.ActiveCount(query, cancellationToken);
            return new GetSubscriptionsResponseModel()
            {
                Data = list,
                Count = count
            };
        }

    }

    public class GetSubscriptionsResponseModel
    {
        public List<SubscriptionDto> Data { get; set; }
        public int Count { get; set; }
    }
}