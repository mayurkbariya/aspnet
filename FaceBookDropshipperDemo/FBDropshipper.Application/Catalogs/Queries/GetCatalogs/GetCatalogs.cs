using System.Linq.Expressions;
using FBDropshipper.Application.Catalogs.Models;
using FBDropshipper.Application.Interfaces;
using FBDropshipper.Application.Shared;
using FBDropshipper.Common.Extensions;
using FBDropshipper.Domain.Entities;
using FBDropshipper.Persistence.Context;
using FBDropshipper.Persistence.Extension;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FBDropshipper.Application.Catalogs.Queries.GetCatalogs;

public class GetCatalogsRequestModel : GetPagedRequest<GetCatalogsResponseModel>
{
    public int MarketPlaceId { get; set; }
    public int CatalogType { get; set; }
}

public class GetCatalogsRequestModelValidator : PageRequestValidator<GetCatalogsRequestModel>
{
    public GetCatalogsRequestModelValidator()
    {
    }
}

public class
    GetCatalogsRequestHandler : IRequestHandler<GetCatalogsRequestModel, GetCatalogsResponseModel>
{
    private readonly ApplicationDbContext _context;
    private readonly ISessionService _sessionService;
    public GetCatalogsRequestHandler(ApplicationDbContext context, ISessionService sessionService)
    {
        _context = context;
        _sessionService = sessionService;
    }

    public async Task<GetCatalogsResponseModel> Handle(GetCatalogsRequestModel request,
        CancellationToken cancellationToken)
    {
        var userId = _sessionService.GetTeamLeaderIdOrUserId();
        Expression<Func<Catalog, bool>> query = p => (p.UserId == userId || p.UserId == null);
        if (request.Search.IsNotNullOrWhiteSpace())
        {
            query = query.AndAlso(p => p.Name.ToLower().Contains(request.Search));
        }

        if (request.CatalogType > 0)
        {
            query = query.AndAlso(p => p.CatalogType == request.CatalogType);
        }

        if (request.MarketPlaceId > 0)
        {
            query = query.AndAlso(p => p.MarketPlaceId == request.MarketPlaceId);
        }

        var list = await _context.Catalogs.GetManyReadOnly(query, request)
            .Select(CatalogSelector.Selector)
            .ToListAsync(cancellationToken);
        var count = await _context.Catalogs.ActiveCount(query, cancellationToken);
        return new GetCatalogsResponseModel()
        {
            Data = list,
            Count = count
        };
    }

}

public class GetCatalogsResponseModel
{
    public List<CatalogDto> Data { get; set; }
    public int Count { get; set; }
}