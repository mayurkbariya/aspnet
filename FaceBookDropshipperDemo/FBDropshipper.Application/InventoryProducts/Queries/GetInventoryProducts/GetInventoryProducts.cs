using System.Linq.Expressions;
using FBDropshipper.Application.InventoryProducts.Models;
using FBDropshipper.Application.Interfaces;
using FBDropshipper.Application.Shared;
using FBDropshipper.Common.Extensions;
using FBDropshipper.Domain.Entities;
using FBDropshipper.Persistence.Context;
using FBDropshipper.Persistence.Extension;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FBDropshipper.Application.InventoryProducts.Queries.GetInventoryProducts;

public class GetInventoryProductsRequestModel : GetPagedRequest<GetInventoryProductsResponseModel>
{
    public int MarketPlaceId { get; set; }
    public int CatalogId { get; set; }
    public int StockStatus { get; set; }
    public float? FromPrice { get; set; }
    public float? ToPrice { get; set; }
    public DateTime? ToCreatedDate { get; set; }
    public DateTime? FromCreatedDate { get; set; }
    public DateTime? ToUpdatedDate { get; set; }
    public DateTime? FromUpdatedDate { get; set; }
}

public class GetInventoryProductsRequestModelValidator : PageRequestValidator<GetInventoryProductsRequestModel>
{
    public GetInventoryProductsRequestModelValidator()
    {

    }
}

public class
    GetInventoryProductsRequestHandler : IRequestHandler<GetInventoryProductsRequestModel, GetInventoryProductsResponseModel>
{
    private readonly ApplicationDbContext _context;
    private readonly ISessionService _sessionService;
    public GetInventoryProductsRequestHandler(ApplicationDbContext context, ISessionService sessionService)
    {
        _context = context;
        _sessionService = sessionService;
    }

    public async Task<GetInventoryProductsResponseModel> Handle(GetInventoryProductsRequestModel request,
        CancellationToken cancellationToken)
    {
        var timeZone = _sessionService.GetTimeZone();
        var userId = _sessionService.GetTeamLeaderIdOrUserId();
        Expression<Func<InventoryProduct, bool>> query = p => p.MarketPlace.Team.UserId == userId;
        if (request.CatalogId > 0)
        {
            query = query.AndAlso(p => p.CatalogProduct.CatalogId == request.CatalogId);
        }
        
        if (request.StockStatus > 0)
        {
            query = query.AndAlso(p => p.StockStatus == request.StockStatus);
        }
        if (request.MarketPlaceId > 0)
        {
            query = query.AndAlso(p => p.MarketPlaceId == request.MarketPlaceId);
        }

        if (request.FromPrice > 0)
        {
            query = query.AndAlso(p => p.Price >= request.FromPrice);
        }
        if (request.ToPrice > 0)
        {
            query = query.AndAlso(p => p.Price <= request.ToPrice);
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

        if (request.Search.IsNotNullOrWhiteSpace())
        {
            query = query.AndAlso(p =>
                p.Title.ToLower().Contains(request.Search) || p.Description.ToLower().Contains(request.Search));
        }
        var list = await _context.InventoryProducts.GetManyReadOnly(query, request)
            .Select(InventoryProductSelector.SelectorDetail)
            .ToListAsync(cancellationToken: cancellationToken);
        var count = await _context.InventoryProducts.ActiveCount(query, cancellationToken);
        return new GetInventoryProductsResponseModel()
        {
            Data = list,
            Count = count
        };
    }

}

public class GetInventoryProductsResponseModel
{
    public List<InventoryProductDetailDto> Data { get; set; }
    public int Count { get; set; }
}