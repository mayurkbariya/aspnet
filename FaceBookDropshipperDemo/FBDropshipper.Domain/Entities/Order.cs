namespace FBDropshipper.Domain.Entities;

public class Order : Base
{
    public int MarketPlaceId { get; set; }
    public MarketPlace MarketPlace { get; set; }
    public int ProductListingId { get; set; }
    public ProductListing ProductListing { get; set; }
    public string OrderId { get; set; }
    public string OrderUrl { get; set; }
    public int Quantity { get; set; }
    public double SubTotal { get; set; }
    public double Shipping { get; set; }
    public double Fee { get; set; }
    public string SupplierOrderId { get; set; }
    public double SupplierCost { get; set; }
    public int TrackingCarrier { get; set; }
    public string TrackingNumber { get; set; }
    public int OrderStatus { get; set; }
}