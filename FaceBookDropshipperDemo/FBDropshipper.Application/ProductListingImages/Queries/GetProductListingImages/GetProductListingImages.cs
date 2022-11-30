using FBDropshipper.Application.ProductListingImages.Models;
using FBDropshipper.Application.InventoryProducts.Models;
using FBDropshipper.Application.Extensions;
using FBDropshipper.Application.Interfaces;
using FBDropshipper.Persistence.Context;
using FBDropshipper.Persistence.Extension;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FBDropshipper.Application.ProductListingImages.Queries.GetProductListingImages;

public class GetProductListingImagesRequestModel : IRequest<GetProductListingImagesResponseModel>
{
    public int Id { get; set; }
}

public class GetProductListingImagesRequestModelValidator : AbstractValidator<GetProductListingImagesRequestModel>
{
    public GetProductListingImagesRequestModelValidator()
    {
        RuleFor(p => p.Id).Required();
    }
}

public class
    GetProductListingImagesRequestHandler : IRequestHandler<GetProductListingImagesRequestModel,
        GetProductListingImagesResponseModel>
{
    private readonly ApplicationDbContext _context;
    private readonly ISessionService _sessionService;
    public GetProductListingImagesRequestHandler(ApplicationDbContext context, ISessionService sessionService)
    {
        _context = context;
        _sessionService = sessionService;
    }

    public async Task<GetProductListingImagesResponseModel> Handle(GetProductListingImagesRequestModel request,
        CancellationToken cancellationToken)
    {
        var userId = _sessionService.GetTeamLeaderIdOrUserId();
        var products = await _context.ProductListingImages
            .GetAllReadOnly(p => p.Id == request.Id &&
                                 (p.ProductListing.MarketPlace.Team.UserId == userId))
            .Select(ProductListingImageSelector.Selector)
            .ToListAsync(cancellationToken: cancellationToken);
        return new GetProductListingImagesResponseModel()
        {
            Data = products
        };
    }

}

public class GetProductListingImagesResponseModel
{
    public List<ProductListingImageDto> Data { get; set; }
}