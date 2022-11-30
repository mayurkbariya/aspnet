using System.Linq.Expressions;
using FBDropshipper.Application.CatalogProducts.Models;
using FBDropshipper.Application.CatalogProducts.Queries.GetCatalogProductDetailById;
using FBDropshipper.Application.Exceptions;
using FBDropshipper.Application.Extensions;
using FBDropshipper.Application.Interfaces;
using FBDropshipper.Application.InventoryProducts.Models;
using FBDropshipper.Common.Extensions;
using FBDropshipper.Domain.Entities;
using FBDropshipper.Persistence.Context;
using FBDropshipper.Persistence.Extension;
using FluentValidation;
using MediatR;

namespace FBDropshipper.Application.InventoryProducts.Queries.GetInventoryProductDetailById;

public class GetInventoryProductDetailByIdRequestModel : IRequest<GetInventoryProductDetailByIdResponseModel>
{
    public int Id { get; set; }
}

public class
    GetInventoryProductDetailByIdRequestModelValidator : AbstractValidator<GetInventoryProductDetailByIdRequestModel>
{
    public GetInventoryProductDetailByIdRequestModelValidator()
    {
        RuleFor(p => p.Id).Required();
    }
}

public class
    GetInventoryProductDetailByIdRequestHandler : IRequestHandler<GetInventoryProductDetailByIdRequestModel,
        GetInventoryProductDetailByIdResponseModel>
{
    private readonly ApplicationDbContext _context;
    private readonly ISessionService _sessionService;

    public GetInventoryProductDetailByIdRequestHandler(ApplicationDbContext context, ISessionService sessionService)
    {
        _context = context;
        _sessionService = sessionService;
    }

    public async Task<GetInventoryProductDetailByIdResponseModel> Handle(
        GetInventoryProductDetailByIdRequestModel request,
        CancellationToken cancellationToken)
    {
        var userId = _sessionService.GetTeamLeaderIdOrUserId();
        Expression<Func<InventoryProduct, bool>> query = p => 
            p.Id == request.Id && (
                p.CatalogProduct.Catalog.UserId == userId || p.CatalogProduct.Catalog.UserId == null);
        var product = await _context.InventoryProducts.GetByWithSelectAsync(query, InventoryProductSelector.SelectorDetail, cancellationToken: cancellationToken);
        if (product == null)
        {
            throw new NotFoundException(nameof(product));
        }
        return product.CreateCopy<GetInventoryProductDetailByIdResponseModel>();
    }

}

public class GetInventoryProductDetailByIdResponseModel : InventoryProductDetailDto
{

}