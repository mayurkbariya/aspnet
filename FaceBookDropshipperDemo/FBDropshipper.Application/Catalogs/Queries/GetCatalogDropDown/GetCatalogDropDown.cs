using System.Linq.Expressions;
using FBDropshipper.Application.Catalogs.Models;
using FBDropshipper.Application.Interfaces;
using FBDropshipper.Domain.Entities;
using FBDropshipper.Persistence.Context;
using FBDropshipper.Persistence.Extension;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FBDropshipper.Application.Catalogs.Queries.GetCatalogDropDown;

public class GetCatalogDropDownRequestModel : IRequest<GetCatalogDropDownResponseModel>
{
    public int CatalogType { get; set; }

}

public class GetCatalogDropDownRequestModelValidator : AbstractValidator<GetCatalogDropDownRequestModel>
{
    public GetCatalogDropDownRequestModelValidator()
    {

    }
}

public class
    GetCatalogDropDownRequestHandler : IRequestHandler<GetCatalogDropDownRequestModel, GetCatalogDropDownResponseModel>
{
    private readonly ApplicationDbContext _context;
    private readonly ISessionService _sessionService;

    public GetCatalogDropDownRequestHandler(ApplicationDbContext context, ISessionService sessionService)
    {
        _context = context;
        _sessionService = sessionService;
    }

    public async Task<GetCatalogDropDownResponseModel> Handle(GetCatalogDropDownRequestModel request,
        CancellationToken cancellationToken)
    {
        var userId = _sessionService.GetTeamLeaderIdOrUserId();
        Expression<Func<Catalog, bool>> query = p => (p.UserId == userId || p.UserId == null);
        if (request.CatalogType > 0)
        {
            query = query.AndAlso(p => p.CatalogType == request.CatalogType);
        }
        var list = await _context.Catalogs.GetAllReadOnly(query)
            .Select(CatalogSelector.SelectorDropDown)
            .ToListAsync(cancellationToken);
        return new GetCatalogDropDownResponseModel()
        {
            Data = list
        };
    }

}

public class GetCatalogDropDownResponseModel
{
    public List<CatalogDropDownDto> Data { get; set; }
}