using System.Linq.Expressions;
using FBDropshipper.Application.CatalogProductImages.Models;
using FBDropshipper.Domain.Entities;

namespace FBDropshipper.Application.CatalogProducts.Models;

public class CatalogProductDetailDto
{
    public CatalogProductDetailDto()
    {
        
    }
    protected CatalogProductDetailDto(CatalogProduct product)
    {
        Id = product.Id;
        CatalogId = product.CatalogId;
        Title = product.Title;
        Price = product.Price;
        Description = product.Description;
        SkuCode = product.SkuCode;
        Url = product.Url;
        StockStatus = product.StockStatus;
        Stock = product.Stock;
        CreatedDate = product.CreatedDate;
        Catalog = product.Catalog?.Name;
        Images = new List<CatalogProductImageDto>();
        if (product.CatalogProductImages != null && product.CatalogProductImages.Any())
        {
            Images = product.CatalogProductImages.Select(pr => new CatalogProductImageDto()
            {
                Id = pr.Id,
                Url = pr.Url,
                CreatedDate = pr.CreatedDate,
                Order = pr.Order
            }).ToList();
        }
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
    public DateTime CreatedDate { get; set; }
    public List<CatalogProductImageDto> Images { get; set; }
    public double Price { get; set; }
    public bool IsInInventory { get; set; }
}

public class CatalogProductDto
{
    public CatalogProductDto()
    {
    }

    public CatalogProductDto(CatalogProduct product)
    {
        Id = product.Id;
        CatalogId = product.CatalogId;
        Title = product.Title;
        Price = product.Price;
        Description = product.Description;
        SkuCode = product.SkuCode;
        Url = product.Url;
        StockStatus = product.StockStatus;
        Stock = product.Stock;
        CreatedDate = product.CreatedDate;
        Catalog = product.Catalog?.Name;
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
}

public static class CatalogProductSelector
{
    public static Expression<Func<CatalogProduct, CatalogProductDto>> Selector = p => new CatalogProductDto()
    {
        Catalog = p.Catalog.Name,
        Description = p.Description,
        Id = p.Id,
        Stock = p.Stock,
        Title = p.Title,
        Url = p.Url,
        CatalogId = p.CatalogId,
        CreatedDate = p.CreatedDate,
        SkuCode = p.SkuCode,
        StockStatus = p.StockStatus,
        Price = p.Price,
    };

    public static Expression<Func<CatalogProduct, CatalogProductDetailDto>> SelectorDetail = p =>
        new CatalogProductDetailDto()
        {
            Catalog = p.Catalog.Name,
            Description = p.Description,
            Id = p.Id,
            Stock = p.Stock,
            Title = p.Title,
            Url = p.Url,
            CatalogId = p.CatalogId,
            CreatedDate = p.CreatedDate,
            SkuCode = p.SkuCode,
            Price = p.Price,
            StockStatus = p.StockStatus,
            IsInInventory = p.InventoryProducts.Any(),
            Images = p.CatalogProductImages.Select(pr => new CatalogProductImageDto()
            {
                Id = pr.Id,
                Url = pr.Url,
                CreatedDate = pr.CreatedDate,
                Order = pr.Order
            }).ToList()
        };
}