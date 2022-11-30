namespace FBDropshipper.Domain.Entities;

public class InventoryProduct : Base
{
    public int? MarketPlaceId { get; set; }
    public MarketPlace MarketPlace { get; set; }
    public int CatalogProductId { get; set; }
    public CatalogProduct CatalogProduct { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string SkuCode { get; set; }
    public string Url { get; set; }
    public int StockStatus { get; set; }
    public int Stock { get; set; }
    public bool IsTracking { get; set; }
    public ICollection<InventoryProductImage> InventoryProductImages { get; set; }
    public IEnumerable<ProductListing> ProductLists { get; set; }
    public double Price { get; set; }
}