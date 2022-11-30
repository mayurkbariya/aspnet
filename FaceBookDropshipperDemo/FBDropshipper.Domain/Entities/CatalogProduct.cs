namespace FBDropshipper.Domain.Entities;

public class CatalogProduct : Base
{
    public int CatalogId { get; set; }
    public Catalog Catalog { get; set; }
    public string Json { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public double Price { get; set; }
    public string SkuCode { get; set; }
    public string Url { get; set; }
    public int StockStatus { get; set; }
    public int Stock { get; set; }
    public ICollection<InventoryProduct> InventoryProducts { get; set; }
    public ICollection<CatalogProductImage> CatalogProductImages { get; set; }
}