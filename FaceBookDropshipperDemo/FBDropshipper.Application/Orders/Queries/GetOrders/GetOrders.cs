using System.Linq.Expressions;
using FBDropshipper.Application.Extensions;
using FBDropshipper.Application.Interfaces;
using FBDropshipper.Application.Orders.Models;
using FBDropshipper.Application.Shared;
using FBDropshipper.Common.Extensions;
using FBDropshipper.Domain.Entities;
using FBDropshipper.Persistence.Context;
using FBDropshipper.Persistence.Extension;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FBDropshipper.Application.Orders.Queries.GetOrders;

public class GetOrdersRequestModel : GetPagedRequest<GetOrdersResponseModel>
{
    public int ProductListingId { get; set; }
    public int MarketPlaceId { get; set; }
    public int OrderStatus { get; set; }
    public int TrackingCarrier { get; set; }
    public float? FromTotal { get; set; }
    public float? ToTotal { get; set; }
    public DateTime? ToCreatedDate { get; set; }
    public DateTime? FromCreatedDate { get; set; }
    public DateTime? ToUpdatedDate { get; set; }
    public DateTime? FromUpdatedDate { get; set; }
}

public class GetOrdersRequestModelValidator : PageRequestValidator<GetOrdersRequestModel>
{
    public GetOrdersRequestModelValidator()
    {
        RuleFor(p => p.MarketPlaceId).Required();
    }
}

public class
    GetOrdersRequestHandler : IRequestHandler<GetOrdersRequestModel, GetOrdersResponseModel>
{
    private readonly ApplicationDbContext _context;
    private readonly ISessionService _sessionService;
    public GetOrdersRequestHandler(ApplicationDbContext context, ISessionService sessionService)
    {
        _context = context;
        _sessionService = sessionService;
    }

    public async Task<GetOrdersResponseModel> Handle(GetOrdersRequestModel request,
        CancellationToken cancellationToken)
    {
        var userId = _sessionService.GetTeamLeaderIdOrUserId();
        var timeZone = _sessionService.GetTimeZone();
        Expression<Func<Order, bool>> query = p =>
            p.MarketPlace.Team.UserId == userId && p.MarketPlaceId == request.MarketPlaceId;
        if (request.Search.IsNotNullOrWhiteSpace())
        {
            query = query.AndAlso(p => 
                p.OrderId.ToLower().Contains(request.Search) ||
                p.OrderUrl.ToLower().Contains(request.Search) ||
                p.TrackingNumber.ToLower().Contains(request.Search));
        }

        if (request.OrderStatus > 0)
        {
            query = query.AndAlso(p => p.OrderStatus == request.OrderStatus);
        }
        if (request.TrackingCarrier > 0)
        {
            query = query.AndAlso(p => p.TrackingCarrier == request.TrackingCarrier);
        }

        if (request.ProductListingId > 0)
        {
            query = query.AndAlso(p => p.ProductListingId == request.ProductListingId);
        }
        if (request.FromTotal > 0)
        {
            query = query.AndAlso(p => p.SubTotal >= request.FromTotal);
        }
        if (request.ToTotal > 0)
        {
            query = query.AndAlso(p => p.SubTotal <= request.ToTotal);
        }
        if (request.FromCreatedDate > DateTime.MinValue)
        {
            query = query.AndAlso(p => p.CreatedDate <= request.FromCreatedDate.ToDateTimeZoneUtc(timeZone));
        }
        if (request.ToCreatedDate > DateTime.MinValue)
        {
            query = query.AndAlso(p => p.CreatedDate <= request.ToCreatedDate.ToDateTimeZoneUtc(timeZone));
        }
        if (request.FromUpdatedDate > DateTime.MinValue)
        {
            query = query.AndAlso(p => p.UpdatedDate <= request.FromUpdatedDate.ToDateTimeZoneUtc(timeZone));
        }
        if (request.ToUpdatedDate > DateTime.MinValue)
        {
            query = query.AndAlso(p => p.UpdatedDate <= request.ToUpdatedDate.ToDateTimeZoneUtc(timeZone));
        }
        var list = await _context.Orders.GetManyReadOnly(query, request)
            .Select(OrderSelector.Selector)
            .ToListAsync(cancellationToken: cancellationToken);
        var count = await _context.Orders.ActiveCount(query, cancellationToken);
        return new GetOrdersResponseModel()
        {
            Data = list,
            Count = count
        };
    }

}

public class GetOrdersResponseModel
{
    public List<OrderDto> Data { get; set; }
    public int Count { get; set; }
}