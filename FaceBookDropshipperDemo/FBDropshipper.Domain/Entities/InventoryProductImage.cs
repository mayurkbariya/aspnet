namespace FBDropshipper.Domain.Entities;

public class InventoryProductImage : Base
{
    public int InventoryProductId { get; set; }
    public InventoryProduct InventoryProduct { get; set; }
    public string Url { get; set; }
    public int Order { get; set; }
}