namespace FBDropshipper.Domain.Entities;

public class ListingTemplate : Base
{
    public int MarketPlaceId { get; set; }
    public MarketPlace MarketPlace { get; set; }
    public string Name { get; set; }
    public float ProfitPercent { get; set; }
    public int Quantity { get; set; }
    public float ShippingRate { get; set; }
    public int DeliveryMethod { get; set; }
    public string Header { get; set; }
    public IEnumerable<ProductListing> ProductLists { get; set; }
}