using FBDropshipper.Application.Exceptions;
using FBDropshipper.Application.Extensions;
using FBDropshipper.Application.Interfaces;
using FBDropshipper.Application.ProductListings.Models;
using FBDropshipper.Common.Extensions;
using FBDropshipper.Domain.Entities;
using FBDropshipper.Domain.Enum;
using FBDropshipper.Persistence.Context;
using FBDropshipper.Persistence.Extension;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FBDropshipper.Application.ProductListings.Commands.CreateProductListing;

public class CreateProductListingRequestModel : IRequest<CreateProductListingResponseModel>
{
    public int MarketPlaceId { get; set; }
    public int TemplateId { get; set; }
    public int InventoryProductId { get; set; }
    public int CategoryId { get; set; }
}

public class CreateProductListingRequestModelValidator : AbstractValidator<CreateProductListingRequestModel>
{
    public CreateProductListingRequestModelValidator()
    {
        RuleFor(p => p.MarketPlaceId).Required();
        RuleFor(p => p.TemplateId).Required();
        RuleFor(p => p.InventoryProductId).Required();
        RuleFor(p => p.CategoryId).Min(0);
    }
}

public class
    CreateProductListingRequestHandler : IRequestHandler<CreateProductListingRequestModel,
        CreateProductListingResponseModel>
{
    private readonly ApplicationDbContext _context;
    private readonly ISessionService _sessionService;
    private readonly IImageService _imageService;
    public CreateProductListingRequestHandler(ApplicationDbContext context, ISessionService sessionService, IImageService imageService)
    {
        _context = context;
        _sessionService = sessionService;
        _imageService = imageService;
    }

    public async Task<CreateProductListingResponseModel> Handle(CreateProductListingRequestModel request,
        CancellationToken cancellationToken)
    {
        var userId = _sessionService.GetTeamLeaderIdOrUserId();
        if (request.CategoryId > 0)
        {
            var category = await _context.Categories.GetByReadOnlyAsync(p => p.Id == request.CategoryId, cancellationToken: cancellationToken);
            if (category == null)
            {
                throw new NotFoundException(nameof(category));
            }
        }
        
        var template = _context.ListingTemplates.GetByReadOnly(p =>
            p.Id == request.TemplateId && p.MarketPlaceId == request.MarketPlaceId &&
            p.MarketPlace.Team.UserId == userId);
        if (template == null)
        {
            throw new NotFoundException(nameof(template));
        }
        var product = _context.InventoryProducts.GetByReadOnly(p => p.Id == request.InventoryProductId
                                                                    && p.MarketPlace.Team.UserId == userId
                                                                    && p.MarketPlaceId == request.MarketPlaceId,
            p => p.Include(pr => pr.InventoryProductImages));
        if (product == null)
        {
            throw new NotFoundException(nameof(product));
        }

        var productListing = new ProductListing()
        {
            CategoryId = request.CategoryId > 0 ? request.CategoryId : null,
            Description = product.Description,
            Header = template.Header,
            Price = (float)Math.Ceiling(((template.ProfitPercent + 100) / 100) * product.Price),
            Quantity = template.Quantity,
            Title = product.Title,
            DeliveryMethod = template.DeliveryMethod,
            ListingStatus = ListingStatus.Draft.ToInt(),
            ListedAt = null,
            InventoryProductId = request.InventoryProductId,
            MarketPlaceId = request.MarketPlaceId,
            ListingTemplateId = request.TemplateId,
            ProductListingImages = new List<ProductListingImage>(),
            ShippingRate = template.ShippingRate,
        };
        var productImages = product.InventoryProductImages.OrderBy(p => p.Order).ToList();
        var images = new List<string>();
        foreach (var img in productImages)      
        {
            images.Add(await _imageService.DownloadAndSave(img.Url));
        }
        for (int i = 0; i < images.Count; i++)
        {
            productListing.ProductListingImages.Add(new ProductListingImage()
            {
                Url = images[i],
                Order = i + 1
            });
        }
        _context.ProductListings.Add(productListing);
        await _context.SaveChangesAsync(cancellationToken);
        return new CreateProductListingResponseModel(productListing);
    }

}

public class CreateProductListingResponseModel : ProductListingDto
{
    public CreateProductListingResponseModel(ProductListing productListing) : base(productListing)
    {
    }
}