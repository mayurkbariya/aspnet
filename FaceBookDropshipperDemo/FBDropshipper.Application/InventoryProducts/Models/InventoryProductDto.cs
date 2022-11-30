using System.Linq.Expressions;
using FBDropshipper.Application.CatalogProductImages.Models;
using FBDropshipper.Application.CatalogProducts.Models;
using FBDropshipper.Application.InventoryProductImages.Models;
using FBDropshipper.Domain.Entities;

namespace FBDropshipper.Application.InventoryProducts.Models;

public class InventoryProductDetailDto
{
    public InventoryProductDetailDto()
    {
        
    }
    protected InventoryProductDetailDto(InventoryProduct product)
    {
        Id = product.Id;
        if (product.CatalogProduct != null)
        {
            CatalogId = product.CatalogProduct.CatalogId;
            Catalog = product.CatalogProduct.Catalog?.Name;
        }
        Title = product.Title;
        Price = product.Price;
        Description = product.Description;
        SkuCode = product.SkuCode;
        Url = product.Url;
        StockStatus = product.StockStatus;
        Stock = product.Stock;
        CreatedDate = product.CreatedDate;
        UpdatedDate = product.UpdatedDate;
        Images = product.InventoryProductImages.Select(pr => new InventoryProductImageDto()
        {
            Id = pr.Id,
            Url = pr.Url,
            CreatedDate = pr.CreatedDate
        }).ToList();
    }

    public string Catalog { get; set; }
    public int CatalogId { get; set; }
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string SkuCode { get; set; }
    public string Url { get; set; }
    public int StockStatus { get; set; }
    public int Stock { get; set; }
    public int? MarketPlaceId { get; set; }
    public string MarketPlace { get; set; }
    public DateTime CreatedDate { get; set; }
    public List<InventoryProductImageDto> Images { get; set; }
    public double Price { get; set; }
    public bool IsInListing { get; set; }
    public DateTime UpdatedDate { get; set; }
}

public class InventoryProductDto
{
    public InventoryProductDto()
    {
        
    }

    public InventoryProductDto(InventoryProduct product)
    {
        Id = product.Id;
        if (product.CatalogProduct != null)
        {
            CatalogId = product.CatalogProduct.CatalogId;
            Catalog = product.CatalogProduct.Catalog?.Name;
        }
        Title = product.Title;
        Price = product.Price;
        Description = product.Description;
        SkuCode = product.SkuCode;
        Url = product.Url;
        StockStatus = product.StockStatus;
        Stock = product.Stock;
        CreatedDate = product.CreatedDate;
        
    }

    public double Price { get; set; }

    public string Catalog { get; set; }
    public int CatalogId { get; set; }

    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string SkuCode { get; set; }
    public string Url { get; set; }
    public int StockStatus { get; set; }
    public int Stock { get; set; }
    public DateTime CreatedDate { get; set; }
    public int? MarketPlaceId { get; set; }
    public string MarketPlace { get; set; }
    public DateTime UpdatedDate { get; set; }
}

public static class InventoryProductSelector
{
    public static Expression<Func<InventoryProduct, InventoryProductDto>> Selector = p => new InventoryProductDto()
    {
        Catalog = p.CatalogProduct.Catalog.Name,
        Description = p.Description,
        Id = p.Id,
        Stock = p.Stock,
        Title = p.Title,
        Url = p.Url,
        CatalogId = p.CatalogProduct.CatalogId,
        CreatedDate = p.CreatedDate,
        SkuCode = p.SkuCode,
        StockStatus = p.StockStatus,
        MarketPlaceId = p.MarketPlaceId,
        MarketPlace = p.MarketPlace.Name,
        UpdatedDate = p.UpdatedDate,
        Price = p.Price
    };

    public static Expression<Func<InventoryProduct, InventoryProductDetailDto>> SelectorDetail = p =>
        new InventoryProductDetailDto()
        {
            Catalog = p.CatalogProduct.Catalog.Name,
            Description = p.Description,
            Id = p.Id,
            Price = p.Price,
            Stock = p.Stock,
            Title = p.Title,
            Url = p.Url,
            CatalogId = p.CatalogProduct.CatalogId,
            CreatedDate = p.CreatedDate,
            SkuCode = p.SkuCode,
            StockStatus = p.StockStatus,
            MarketPlaceId = p.MarketPlaceId,
            MarketPlace = p.MarketPlace.Name,
            IsInListing = p.ProductLists.Any(),
            UpdatedDate = p.UpdatedDate,
            Images = p.InventoryProductImages.Select(pr => new InventoryProductImageDto()
            {
                Id = pr.Id,
                Url = pr.Url,
                CreatedDate = pr.CreatedDate
            }).ToList()
        };
}