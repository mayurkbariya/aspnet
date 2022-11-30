using System.Linq.Expressions;
using FBDropshipper.Application.Extensions;
using FBDropshipper.Application.Interfaces;
using FBDropshipper.Application.ProductListings.Models;
using FBDropshipper.Application.Shared;
using FBDropshipper.Common.Extensions;
using FBDropshipper.Domain.Entities;
using FBDropshipper.Persistence.Context;
using FBDropshipper.Persistence.Extension;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FBDropshipper.Application.ProductListings.Queries.GetProductListings;

public class GetProductListingsRequestModel : GetPagedRequest<GetProductListingsResponseModel>
{
    public int MarketPlaceId { get; set; }
    public int TemplateId { get; set; }
    public int InventoryProductId { get; set; }
    public int CategoryId { get; set; }
    public int ListingStatus { get; set; }
    public float? FromPrice { get; set; }
    public float? ToPrice { get; set; }
    public DateTime? ToCreatedDate { get; set; }
    public DateTime? FromCreatedDate { get; set; }
    public DateTime? ToUpdatedDate { get; set; }
    public DateTime? FromUpdatedDate { get; set; }

}

public class GetProductListingsRequestModelValidator : PageRequestValidator<GetProductListingsRequestModel>
{
    public GetProductListingsRequestModelValidator()
    {
        RuleFor(p => p.MarketPlaceId).Required();
    }
}

public class
    GetProductListingsRequestHandler : IRequestHandler<GetProductListingsRequestModel, GetProductListingsResponseModel>
{
    private readonly ApplicationDbContext _context;
    private readonly ISessionService _sessionService;
    public GetProductListingsRequestHandler(ApplicationDbContext context, ISessionService sessionService)
    {
        _context = context;
        _sessionService = sessionService;
    }

    public async Task<GetProductListingsResponseModel> Handle(GetProductListingsRequestModel request,
        CancellationToken cancellationToken)
    {
        var timeZone = _sessionService.GetTimeZone();
        var userId = _sessionService.GetTeamLeaderIdOrUserId();
        Expression<Func<ProductListing, bool>> query = p => p.MarketPlaceId == request.MarketPlaceId
            && p.MarketPlace.Team.UserId == userId;
        if (request.TemplateId > 0)
        {
            query = query.AndAlso(p => p.ListingTemplateId == request.TemplateId);
        }
        if (request.InventoryProductId > 0)
        {
            query = query.AndAlso(p => p.InventoryProductId == request.InventoryProductId);
        }
        if (request.CategoryId > 0)
        {
            query = query.AndAlso(p => p.CategoryId == request.CategoryId);
        }
        if (request.ListingStatus > 0)
        {
            query = query.AndAlso(p => p.ListingStatus == request.ListingStatus);
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
        var list = await _context.ProductListings.GetManyReadOnly(query, request)
            .Select(ProductListingSelector.SelectorDetail)
            .ToListAsync(cancellationToken);
        var count = await _context.ProductListings.ActiveCount(query, cancellationToken);
        return new GetProductListingsResponseModel()
        {
            Data = list,
            Count = count
        };
    }

}

public class GetProductListingsResponseModel
{
    public List<ProductListingDetailDto> Data { get; set; }
    public int Count { get; set; }
}