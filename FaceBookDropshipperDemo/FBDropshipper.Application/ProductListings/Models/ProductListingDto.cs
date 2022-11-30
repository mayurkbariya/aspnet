using System.Linq.Expressions;
using FBDropshipper.Application.ProductListingImages.Models;
using FBDropshipper.Domain.Entities;

namespace FBDropshipper.Application.ProductListings.Models;

public class ProductListingDetailDto : ProductListingDto
{
    public ProductListingDetailDto()
    {
        
    }

    public ProductListingDetailDto(ProductListing listing) : base(listing)
    {
        Images = new List<ProductListingImageDto>();
        if (listing.ProductListingImages != null)
        {
            Images.AddRange(listing.ProductListingImages.Select(p => new ProductListingImageDto()
            {
                Id = p.Id,
                Url = p.Url,
                CreatedDate = p.CreatedDate
            }));    
        }
    }
    public List<ProductListingImageDto> Images { get; set; }
}

public class ProductListingDto
{
    public ProductListingDto()
    {
        
    }

    public ProductListingDto(ProductListing listing)
    {
        Id = listing.Id;
        CreatedDate = listing.CreatedDate;
        ListingTemplateId = listing.ListingTemplateId;
        ListingTemplate = listing.ListingTemplate?.Name;
        MarketPlaceId = listing.MarketPlaceId;
        MarketPlace = listing.MarketPlace?.Name;
        InventoryProductId = listing.InventoryProductId;
        Title = listing.Title;
        Description = listing.Description;
        Header = listing.Header;
        Price = listing.Price;
        Quantity = listing.Quantity;
        CategoryId = listing.CategoryId;
        Category = listing.Category?.Name;
        ListingId = listing.ListingId;
        ListingUrl = listing.ListingUrl;
        ListedAt = listing.ListedAt;
        DeliveryMethod = listing.DeliveryMethod;
        ListingStatus = listing.ListingStatus;
        ShippingRate = listing.ShippingRate;
        UpdatedDate = listing.UpdatedDate;
    }

    public double ShippingRate { get; set; }
    public DateTime UpdatedDate { get; set; }

    public DateTime CreatedDate { get; set; }
    public int Id { get; set; }
    public int? ListingTemplateId { get; set; }
    public string ListingTemplate { get; set; }
    public int MarketPlaceId { get; set; }
    public string MarketPlace { get; set; }
    public int InventoryProductId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Header { get; set; }
    public float Price { get; set; }
    public int Quantity { get; set; }
    public int? CategoryId { get; set; }
    public string Category { get; set; }
    public string ListingId { get; set; }
    public string ListingUrl { get; set; }
    public DateTime? ListedAt { get; set; }
    public int DeliveryMethod { get; set; }
    public int ListingStatus { get; set; }
}

public static class ProductListingSelector
{
    public static readonly Expression<Func<ProductListing, ProductListingDetailDto>> SelectorDetail = p => new ProductListingDetailDto()
    {
        Category = p.Category.Name,
        Description = p.Description,
        Header = p.Header,
        Id = p.Id,
        Price = p.Price,
        Quantity = p.Quantity,
        Title = p.Title,
        CategoryId = p.CategoryId,
        CreatedDate = p.CreatedDate,
        DeliveryMethod = p.DeliveryMethod,
        ListedAt = p.ListedAt,
        ListingId = p.ListingId,
        ListingStatus = p.ListingStatus,
        ListingTemplate = p.ListingTemplate.Name,
        ListingUrl = p.ListingUrl,
        MarketPlace = p.MarketPlace.Name,
        InventoryProductId = p.InventoryProductId,
        ListingTemplateId = p.ListingTemplateId,
        MarketPlaceId = p.MarketPlaceId,
        ShippingRate = p.ShippingRate,
        UpdatedDate = p.UpdatedDate,
        Images = p.ProductListingImages.Select(pr => new ProductListingImageDto()
        {
            Id = pr.Id,
            Url = pr.Url,
            CreatedDate = pr.CreatedDate
        }).ToList()
    };
    public static Expression<Func<ProductListing, ProductListingDto>> Selector = p => new ProductListingDto()
    {
        Category = p.Category.Name,
        Description = p.Description,
        Header = p.Header,
        Id = p.Id,
        Price = p.Price,
        Quantity = p.Quantity,
        Title = p.Title,
        CategoryId = p.CategoryId,
        CreatedDate = p.CreatedDate,
        DeliveryMethod = p.DeliveryMethod,
        ListedAt = p.ListedAt,
        ListingId = p.ListingId,
        ListingStatus = p.ListingStatus,
        ListingTemplate = p.ListingTemplate.Name,
        ListingUrl = p.ListingUrl,
        MarketPlace = p.MarketPlace.Name,
        InventoryProductId = p.InventoryProductId,
        ListingTemplateId = p.ListingTemplateId,
        ShippingRate = p.ShippingRate,
        UpdatedDate = p.UpdatedDate,
        MarketPlaceId = p.MarketPlaceId
    };
}