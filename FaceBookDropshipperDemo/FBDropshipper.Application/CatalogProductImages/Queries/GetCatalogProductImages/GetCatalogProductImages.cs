using FBDropshipper.Application.CatalogProductImages.Models;
using FBDropshipper.Application.CatalogProducts.Models;
using FBDropshipper.Application.Extensions;
using FBDropshipper.Application.Interfaces;
using FBDropshipper.Persistence.Context;
using FBDropshipper.Persistence.Extension;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FBDropshipper.Application.CatalogProductImages.Queries.GetCatalogProductImages;

public class GetCatalogProductImagesRequestModel : IRequest<GetCatalogProductImagesResponseModel>
{
    public int Id { get; set; }
}

public class GetCatalogProductImagesRequestModelValidator : AbstractValidator<GetCatalogProductImagesRequestModel>
{
    public GetCatalogProductImagesRequestModelValidator()
    {
        RuleFor(p => p.Id).Required();
    }
}

public class
    GetCatalogProductImagesRequestHandler : IRequestHandler<GetCatalogProductImagesRequestModel,
        GetCatalogProductImagesResponseModel>
{
    private readonly ApplicationDbContext _context;
    private readonly ISessionService _sessionService;
    public GetCatalogProductImagesRequestHandler(ApplicationDbContext context, ISessionService sessionService)
    {
        _context = context;
        _sessionService = sessionService;
    }

    public async Task<GetCatalogProductImagesResponseModel> Handle(GetCatalogProductImagesRequestModel request,
        CancellationToken cancellationToken)
    {
        var userId = _sessionService.GetTeamLeaderIdOrUserId();
        var products = await _context.CatalogProductImages
            .GetAllReadOnly(p => p.Id == request.Id &&
                                 (p.CatalogProduct.Catalog.UserId == userId ||
                                  p.CatalogProduct.Catalog.UserId == null))
            .Select(CatalogProductImageSelector.Selector)
            .ToListAsync(cancellationToken: cancellationToken);
        return new GetCatalogProductImagesResponseModel()
        {
            Data = products
        };
    }

}

public class GetCatalogProductImagesResponseModel
{
    public List<CatalogProductImageDto> Data { get; set; }
}