namespace FBDropshipper.Domain.Entities;

public class ProductListing : Base
{
    public int? ListingTemplateId { get; set; }
    public ListingTemplate ListingTemplate { get; set; }
    public int MarketPlaceId { get; set; }
    public MarketPlace MarketPlace { get; set; }
    public int InventoryProductId { get; set; }
    public InventoryProduct InventoryProduct { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Header { get; set; }
    public float Price { get; set; }
    public double ShippingRate { get; set; }
    public int Quantity { get; set; }
    public int? CategoryId { get; set; }
    public Category Category { get; set; }
    public string ListingId { get; set; }
    public string ListingUrl { get; set; }
    public DateTime? ListedAt { get; set; }
    public int DeliveryMethod { get; set; }
    public int ListingStatus { get; set; }
    public ICollection<ProductListingImage> ProductListingImages { get; set; }
    public IEnumerable<Order> Orders { get; set; }
}