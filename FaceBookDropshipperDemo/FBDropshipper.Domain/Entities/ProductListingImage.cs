namespace FBDropshipper.Domain.Entities;

public class ProductListingImage : Base
{
    public int ProductListingId { get; set; }
    public ProductListing ProductListing { get; set; }
    public string Url { get; set; }
    public int Order { get; set; }
}