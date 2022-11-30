namespace FBDropshipper.Domain.Entities;

public class Category : Base
{
    public string Name { get; set; }
    public int CategoryType { get; set; }
    public IEnumerable<ProductListing> ProductLists { get; set; }
}