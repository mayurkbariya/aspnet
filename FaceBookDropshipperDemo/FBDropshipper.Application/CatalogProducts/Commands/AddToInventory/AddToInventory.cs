using FBDropshipper.Application.Exceptions;
using FBDropshipper.Application.Extensions;
using FBDropshipper.Application.Interfaces;
using FBDropshipper.Domain.Entities;
using FBDropshipper.Persistence.Context;
using FBDropshipper.Persistence.Extension;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FBDropshipper.Application.CatalogProducts.Commands.AddToInventory;

public class AddToInventoryRequestModel : IRequest<AddToInventoryResponseModel>
{
    public int CatalogProductId { get; set; }
    public int MarketPlaceId { get; set; }
}

public class AddToInventoryRequestModelValidator : AbstractValidator<AddToInventoryRequestModel>
{
    public AddToInventoryRequestModelValidator()
    {
        RuleFor(p => p.CatalogProductId).Required();
        RuleFor(p => p.MarketPlaceId).Required();
    }
}

public class
    AddToInventoryRequestHandler : IRequestHandler<AddToInventoryRequestModel, AddToInventoryResponseModel>
{
    private readonly ApplicationDbContext _context;
    private readonly IImageService _imageService;
    public AddToInventoryRequestHandler(ApplicationDbContext context, ISessionService sessionService, IImageService imageService)
    {
        _context = context;
        _imageService = imageService;
    }

    public async Task<AddToInventoryResponseModel> Handle(AddToInventoryRequestModel request,
        CancellationToken cancellationToken)
    {
        
        var product = await _context.CatalogProducts.GetByReadOnlyAsync(p => p.Id == request.CatalogProductId, p => p.Include(pr => pr.CatalogProductImages), cancellationToken: cancellationToken);
        if (product == null)
        {
            throw new NotFoundException(nameof(product));
        }
        var catalogProduct = await _context.InventoryProducts.ActiveAny(p => 
            p.MarketPlaceId == request.MarketPlaceId &&
            p.CatalogProductId == request.CatalogProductId);
        if (catalogProduct)
        {
            throw new AlreadyExistsException(nameof(product));
        }
        
        var images = new List<string>();
        var productImages = product.CatalogProductImages.OrderBy(p => p.Order).ToList();
        foreach (var img in productImages)      
        {
            images.Add(await _imageService.DownloadAndSave(img.Url));
        }
        var inventoryProduct = new InventoryProduct()
        {
            MarketPlaceId = request.MarketPlaceId,
            Description = product.Description,
            Stock = product.Stock,
            Title = product.Title,
            Url = product.Url,
            CatalogProductId = product.Id,
            IsTracking = false,
            SkuCode = product.SkuCode,
            StockStatus = product.StockStatus,
            InventoryProductImages = new List<InventoryProductImage>(),
            Price = product.Price
        };
        for (int i = 0; i < images.Count; i++)
        {
            inventoryProduct.InventoryProductImages.Add(new InventoryProductImage()
            {
                Url = images[i],
                Order = i + 1
            });
        }
        _context.InventoryProducts.Add(inventoryProduct);
        await _context.SaveChangesAsync(cancellationToken);
        return new AddToInventoryResponseModel();
    }

}

public class AddToInventoryResponseModel
{

}