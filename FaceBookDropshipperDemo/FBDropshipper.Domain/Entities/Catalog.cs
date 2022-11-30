namespace FBDropshipper.Domain.Entities;

public class Catalog : Base
{
    public int? MarketPlaceId { get; set; }
    public MarketPlace MarketPlace { get; set; }
    public string UserId { get; set; }
    public User User { get; set; }
    public string Name { get; set; }
    public int CatalogType { get; set; }
    public bool CanBeDeleted { get; set; } = true;
    public IEnumerable<CatalogProduct> CatalogProducts { get; set; }
    public IEnumerable<InventoryProduct> InventoryProducts { get; set; }
}