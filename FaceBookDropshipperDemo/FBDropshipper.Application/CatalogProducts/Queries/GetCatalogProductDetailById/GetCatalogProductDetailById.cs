using System.Linq.Expressions;
using FBDropshipper.Application.CatalogProducts.Models;
using FBDropshipper.Application.Exceptions;
using FBDropshipper.Application.Extensions;
using FBDropshipper.Application.Interfaces;
using FBDropshipper.Common.Extensions;
using FBDropshipper.Domain.Entities;
using FBDropshipper.Persistence.Context;
using FBDropshipper.Persistence.Extension;
using FluentValidation;
using MediatR;

namespace FBDropshipper.Application.CatalogProducts.Queries.GetCatalogProductDetailById;

public class GetCatalogProductDetailByIdRequestModel : IRequest<GetCatalogProductDetailByIdResponseModel>
{
    public int Id { get; set; }
}

public class
    GetCatalogProductDetailByIdRequestModelValidator : AbstractValidator<GetCatalogProductDetailByIdRequestModel>
{
    public GetCatalogProductDetailByIdRequestModelValidator()
    {
        RuleFor(p => p.Id).Required();
    }
}

public class
    GetCatalogProductDetailByIdRequestHandler : IRequestHandler<GetCatalogProductDetailByIdRequestModel,
        GetCatalogProductDetailByIdResponseModel>
{
    private readonly ApplicationDbContext _context;
    private readonly ISessionService _sessionService;
    public GetCatalogProductDetailByIdRequestHandler(ApplicationDbContext context, ISessionService sessionService)
    {
        _context = context;
        _sessionService = sessionService;
    }

    public async Task<GetCatalogProductDetailByIdResponseModel> Handle(GetCatalogProductDetailByIdRequestModel request,
        CancellationToken cancellationToken)
    {
        var userId = _sessionService.GetTeamLeaderIdOrUserId();
        Expression<Func<CatalogProduct, bool>> query = p => 
            p.Id == request.Id && (
            p.Catalog.UserId == userId || p.Catalog.UserId == null);
        var product = await _context.CatalogProducts.GetByWithSelectAsync(query, CatalogProductSelector.SelectorDetail, cancellationToken: cancellationToken);
        if (product == null)
        {
            throw new NotFoundException(nameof(product));
        }

        return product.CreateCopy<GetCatalogProductDetailByIdResponseModel>();
    }

}

public class GetCatalogProductDetailByIdResponseModel : CatalogProductDetailDto
{

}