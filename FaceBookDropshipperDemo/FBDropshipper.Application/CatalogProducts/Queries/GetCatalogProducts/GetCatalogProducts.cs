using System.Linq.Expressions;
using FBDropshipper.Application.CatalogProducts.Models;
using FBDropshipper.Application.Interfaces;
using FBDropshipper.Application.Shared;
using FBDropshipper.Common.Extensions;
using FBDropshipper.Domain.Entities;
using FBDropshipper.Persistence.Context;
using FBDropshipper.Persistence.Extension;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TimeZoneConverter;

namespace FBDropshipper.Application.CatalogProducts.Queries.GetCatalogProducts;

public class GetCatalogProductsRequestModel : GetPagedRequest<GetCatalogProductsResponseModel>
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

public class GetCatalogProductsRequestModelValidator : PageRequestValidator<GetCatalogProductsRequestModel>
{
    public GetCatalogProductsRequestModelValidator()
    {

    }
}

public class
    GetCatalogProductsRequestHandler : IRequestHandler<GetCatalogProductsRequestModel, GetCatalogProductsResponseModel>
{
    private readonly ApplicationDbContext _context;
    private readonly ISessionService _sessionService;
    public GetCatalogProductsRequestHandler(ApplicationDbContext context, ISessionService sessionService)
    {
        _context = context;
        _sessionService = sessionService;
    }

    public async Task<GetCatalogProductsResponseModel> Handle(GetCatalogProductsRequestModel request,
        CancellationToken cancellationToken)
    {
        var timeZone = _sessionService.GetTimeZone();
        var userId = _sessionService.GetTeamLeaderIdOrUserId();
        Expression<Func<CatalogProduct, bool>> query = p => p.Catalog.UserId == userId || p.Catalog.UserId == null;
        if (request.CatalogId > 0)
        {
            query = query.AndAlso(p => p.CatalogId == request.CatalogId);
        }

        if (request.Search.IsNotNullOrWhiteSpace())
        {
            query = query.AndAlso(p => p.Catalog.Name.ToLower().Contains(request.Search) ||
                                       p.Title.ToLower().Contains(request.Search) ||
                                       p.Description.ToLower().Contains(request.Search)
            );
        }
        if (request.StockStatus > 0)
        {
            query = query.AndAlso(p => p.StockStatus == request.StockStatus);
        }
        if (request.MarketPlaceId > 0)
        {
            query = query.AndAlso(p => p.Catalog.MarketPlaceId == request.MarketPlaceId);
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

        var list = await _context.CatalogProducts.GetManyReadOnly(query, request)
            .Select(CatalogProductSelector.SelectorDetail)
            .ToListAsync(cancellationToken: cancellationToken);
        var count = await _context.CatalogProducts.ActiveCount(query, cancellationToken);
        return new GetCatalogProductsResponseModel()
        {
            Data = list,
            Count = count
        };
    }

}

public class GetCatalogProductsResponseModel
{
    public List<CatalogProductDetailDto> Data { get; set; }
    public int Count { get; set; }
}