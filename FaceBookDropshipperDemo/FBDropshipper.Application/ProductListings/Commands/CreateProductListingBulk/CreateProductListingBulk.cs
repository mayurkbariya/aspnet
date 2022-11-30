using FBDropshipper.Application.Exceptions;
using FBDropshipper.Application.Extensions;
using FBDropshipper.Application.Interfaces;
using FBDropshipper.Application.ListingTemplates.Models;
using FBDropshipper.Common.Extensions;
using FBDropshipper.Domain.Entities;
using FBDropshipper.Domain.Enum;
using FBDropshipper.Persistence.Context;
using FBDropshipper.Persistence.Extension;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FBDropshipper.Application.ProductListings.Commands.CreateProductListingBulk;

public class CreateProductListingBulkRequestModel : IRequest<CreateProductListingBulkResponseModel>
{
    public ListingTemplateDto Template { get; set; }
    public int MarketPlaceId { get; set; }
    public int TemplateId { get; set; }
    public int[] InventoryProductIds { get; set; }
    public int CategoryId { get; set; }
}

public class CreateProductListingBulkRequestModelValidator : AbstractValidator<CreateProductListingBulkRequestModel>
{
    public CreateProductListingBulkRequestModelValidator()
    {
        RuleFor(p => p.MarketPlaceId).Required();
        RuleFor(p => p.TemplateId).Required();
        RuleFor(p => p.InventoryProductIds).Required().Max(50);
        RuleFor(p => p.CategoryId).Min(0);
    }
}

public class
    CreateProductListingBulkRequestHandler : IRequestHandler<CreateProductListingBulkRequestModel,
        CreateProductListingBulkResponseModel>
{
    private readonly ApplicationDbContext _context;
    private readonly IImageService _imageService;
    public CreateProductListingBulkRequestHandler(ApplicationDbContext context, IImageService imageService)
    {
        _context = context;
        _imageService = imageService;
    }

    public async Task<CreateProductListingBulkResponseModel> Handle(CreateProductListingBulkRequestModel request,
        CancellationToken cancellationToken)
    {
        var products = await _context.InventoryProducts.GetAllReadOnly(p => request.InventoryProductIds.Contains(p.Id) 
                                                                    && p.MarketPlaceId == request.MarketPlaceId)
            .Include(pr => pr.InventoryProductImages)
            .ToListAsync(cancellationToken);
        if (products.Count == 0)
        {
            throw new NotFoundException(nameof(products));
        }
        foreach (var product in products)
        {
            var productListing = new ProductListing()
            {
                CategoryId = request.CategoryId > 0 ? request.CategoryId : null,
                Description = product.Description,
                Header = request.Template.Header,
                Price = (float)Math.Ceiling((request.Template.ProfitPercent / 100) * product.Price),
                Quantity = request.Template.Quantity,
                Title = product.Title,
                DeliveryMethod = request.Template.DeliveryMethod,
                ListingStatus = ListingStatus.Draft.ToInt(),
                ListedAt = null,
                InventoryProductId = product.Id,
                MarketPlaceId = request.MarketPlaceId,
                ListingTemplateId = request.TemplateId,
                ProductListingImages = new List<ProductListingImage>(),
                ShippingRate = request.Template.ShippingRate,
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
        }
        await _context.SaveChangesAsync(cancellationToken);
        return new CreateProductListingBulkResponseModel();
    }

}

public class CreateProductListingBulkResponseModel
{

}