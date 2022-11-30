using FBDropshipper.Application.CatalogProducts.Models;
using FBDropshipper.Application.Exceptions;
using FBDropshipper.Application.Extensions;
using FBDropshipper.Application.Interfaces;
using FBDropshipper.Common.Extensions;
using FBDropshipper.Domain.Entities;
using FBDropshipper.Domain.Enum;
using FBDropshipper.Persistence.Context;
using FBDropshipper.Persistence.Extension;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace FBDropshipper.Application.CatalogProducts.Commands.ImportCatalogProductBySku;

public class ImportCatalogProductBySkuRequestModel : IRequest<ImportCatalogProductBySkuResponseModel>
{
    public string SkuCode { get; set; }
}

public class ImportCatalogProductBySkuRequestModelValidator : AbstractValidator<ImportCatalogProductBySkuRequestModel>
{
    public ImportCatalogProductBySkuRequestModelValidator()
    {
        RuleFor(p => p.SkuCode).Required();
    }
}

public class
    ImportCatalogProductBySkuRequestHandler : IRequestHandler<ImportCatalogProductBySkuRequestModel,
        ImportCatalogProductBySkuResponseModel>
{
    private readonly ApplicationDbContext _context;
    private readonly IRainforestApiService _rainforestApiService;
    private readonly IImageService _imageService;

    public ImportCatalogProductBySkuRequestHandler(ApplicationDbContext context,
        IRainforestApiService rainforestApiService, IImageService imageService)
    {
        _context = context;
        _rainforestApiService = rainforestApiService;
        _imageService = imageService;
    }

    public async Task<ImportCatalogProductBySkuResponseModel> Handle(ImportCatalogProductBySkuRequestModel request,
        CancellationToken cancellationToken)
    {
        var product =
            await _context.CatalogProducts.GetByReadOnlyAsync(p =>
                    p.SkuCode == request.SkuCode && p.Catalog.UserId == null,
                p => p.Include(pr => pr.Catalog),
                cancellationToken: cancellationToken);
        if (product == null)
        {
            var catalog = _context.Catalogs.GetByReadOnly(p => p.UserId == null);
            if (catalog == null)
            {
                throw new NotFoundException(nameof(catalog));
            }

            var rainforestResponse = await _rainforestApiService.GetProductBySku(request.SkuCode);
            if (rainforestResponse == null)
            {
                throw new BadRequestException($"Error while fetching Product By {catalog.Name}");
            }

            if (!rainforestResponse.RequestInfo.Success)
            {
                throw new BadRequestException($"Error while fetching Product By {catalog.Name}");
            }

            var description = "";
            rainforestResponse.Product.FeatureBullets?.ForEach(p => { description += p + "\n"; });
            var images = new List<string>();
            if (rainforestResponse.Product.MainImage != null &&
                rainforestResponse.Product.MainImage.Link.IsNotNullOrWhiteSpace())
            {
                var newUrl = await _imageService.DownloadAndSave(rainforestResponse.Product.MainImage.Link);
                images.Add(newUrl);
            }

            if (rainforestResponse.Product.Images.Any())
            {
                foreach (var image in rainforestResponse.Product.Images)
                {
                    if (images.Count == 10)
                    {
                        continue;
                    }
                    var newUrl = await _imageService.DownloadAndSave(image.Link);
                    if (newUrl.IsNotNullOrWhiteSpace())
                    {
                        images.Add(newUrl);
                    }
                }
            }

            var price = 0d;
            StockStatus status = StockStatus.OutOfStock;
            if (rainforestResponse.Product?.Variants?.FirstOrDefault(p => p.IsCurrentProduct)?.Price != null)
            {
                price = rainforestResponse.Product.Variants.First(p => p.IsCurrentProduct).Price.Value;
            }
            else if (rainforestResponse.Product?.BuyboxWinner?.Price != null)
            {
                price = rainforestResponse.Product.BuyboxWinner.Price.Value;
            }

            if (rainforestResponse.Product?.BuyboxWinner?.Price is { Value: > 0 })
            {
                status = StockStatus.InStock;
            }

            if (price == 0)
            {
                throw new ThirdPartyException("Cannot add Product as Price is 0 for SKU " + request.SkuCode);
            }
            var newProduct = new CatalogProduct()
            {
                CatalogId = catalog.Id,
                Description = description,
                Title = rainforestResponse.Product.Title,
                Json = JsonConvert.SerializeObject(rainforestResponse),
                Stock = 0,
                SkuCode = request.SkuCode,
                Url = rainforestResponse.Product.Link,
                StockStatus =
                    status.ToInt(),
                CatalogProductImages = new List<CatalogProductImage>(),
                Price = price,
            };
            for (int i = 0; i < images.Count; i++)
            {
                newProduct.CatalogProductImages.Add(new CatalogProductImage()
                {
                    Url = images[i],
                    Order = i + 1
                });
            }

            _context.CatalogProducts.Add(newProduct);
            await _context.SaveChangesAsync(cancellationToken);
            return new ImportCatalogProductBySkuResponseModel(newProduct);
        }
        return new ImportCatalogProductBySkuResponseModel(product);
    }
}

public class ImportCatalogProductBySkuResponseModel : CatalogProductDto
{
    public ImportCatalogProductBySkuResponseModel(CatalogProduct product) : base(product)
    {
    }
}