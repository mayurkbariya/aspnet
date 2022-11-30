using FBDropshipper.Application.InventoryProductImages.Models;
using FBDropshipper.Application.InventoryProducts.Models;
using FBDropshipper.Application.Extensions;
using FBDropshipper.Application.Interfaces;
using FBDropshipper.Persistence.Context;
using FBDropshipper.Persistence.Extension;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FBDropshipper.Application.InventoryProductImages.Queries.GetInventoryProductImages;

public class GetInventoryProductImagesRequestModel : IRequest<GetInventoryProductImagesResponseModel>
{
    public int Id { get; set; }
}

public class GetInventoryProductImagesRequestModelValidator : AbstractValidator<GetInventoryProductImagesRequestModel>
{
    public GetInventoryProductImagesRequestModelValidator()
    {
        RuleFor(p => p.Id).Required();
    }
}

public class
    GetInventoryProductImagesRequestHandler : IRequestHandler<GetInventoryProductImagesRequestModel,
        GetInventoryProductImagesResponseModel>
{
    private readonly ApplicationDbContext _context;
    private readonly ISessionService _sessionService;
    public GetInventoryProductImagesRequestHandler(ApplicationDbContext context, ISessionService sessionService)
    {
        _context = context;
        _sessionService = sessionService;
    }

    public async Task<GetInventoryProductImagesResponseModel> Handle(GetInventoryProductImagesRequestModel request,
        CancellationToken cancellationToken)
    {
        var userId = _sessionService.GetTeamLeaderIdOrUserId();
        var products = await _context.InventoryProductImages
            .GetAllReadOnly(p => p.Id == request.Id &&
                                 (p.InventoryProduct.CatalogProduct.Catalog.UserId == userId ||
                                  p.InventoryProduct.CatalogProduct.Catalog.UserId == null))
            .Select(InventoryProductImageSelector.Selector)
            .ToListAsync(cancellationToken: cancellationToken);
        return new GetInventoryProductImagesResponseModel()
        {
            Data = products
        };
    }

}

public class GetInventoryProductImagesResponseModel
{
    public List<InventoryProductImageDto> Data { get; set; }
}