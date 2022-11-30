namespace FBDropshipper.Domain.Entities
{
    public class MarketPlace : Base
    {
        public string Name { get; set; }
        public int TeamId { get; set; }
        public Team Team { get; set; }
        public int MarketPlaceType { get; set; }
        public IEnumerable<Catalog> Catalogs { get; set; }
        public IEnumerable<InventoryProduct> InventoryProducts { get; set; }
        public IEnumerable<ListingTemplate> ListingTemplates { get; set; }
        public IEnumerable<ProductListing> ProductLists { get; set; }
        public IEnumerable<TeamMemberPermission> TeamMemberPermissions { get; set; }
        public IEnumerable<Order> Orders { get; set; }
    }
}