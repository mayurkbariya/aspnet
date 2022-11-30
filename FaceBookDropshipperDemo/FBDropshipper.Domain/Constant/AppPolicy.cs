namespace FBDropshipper.Domain.Constant;

public static class AppPolicy
{
    public static class Catalogs
    {
        public const string Insert = "Policy.Catalog.Insert";
        public const string View = "Policy.Catalog.View";
        public const string Update = "Policy.Catalog.Update";
        public const string Delete = "Policy.Catalog.Delete";

        public static readonly string[] All =
        {
            Insert,
            View,
            Update,
            Delete,
        };
    }
    public static class CatalogProducts
    {
        public const string Insert = "Policy.CatalogProduct.Insert";
        public const string View = "Policy.CatalogProduct.View";
        public const string Update = "Policy.CatalogProduct.Update";
        public const string Delete = "Policy.CatalogProduct.Delete";
        public const string AddToInventory = "Policy.AddToInventory.Delete";
        public const string Import = "Policy.Import.Delete";

        public static readonly string[] All =
        {
            Insert,
            View,
            Update,
            Delete,
            AddToInventory,
            Import
        };
    }
    public static class CatalogProductImages
    {
        public const string Insert = "Policy.CatalogProductImage.Insert";
        public const string View = "Policy.CatalogProductImage.View";
        public const string Update = "Policy.CatalogProductImage.Update";
        public const string Delete = "Policy.CatalogProductImage.Delete";

        public static readonly string[] All =
        {
            Insert,
            View,
            Update,
            Delete,
        };
    }
    public static class InventoryProducts
    {
        public const string Insert = "Policy.InventoryProduct.Insert";
        public const string View = "Policy.InventoryProduct.View";
        public const string Update = "Policy.InventoryProduct.Update";
        public const string Delete = "Policy.InventoryProduct.Delete";

        public static readonly string[] All =
        {
            Insert,
            View,
            Update,
            Delete,
        };
    }
    public static class InventoryProductImages
    {
        public const string Insert = "Policy.InventoryProductImage.Insert";
        public const string View = "Policy.InventoryProductImage.View";
        public const string Update = "Policy.InventoryProductImage.Update";
        public const string Delete = "Policy.InventoryProductImage.Delete";
           
        public static readonly string[] All =
        {
            Insert,
            View,
            Update,
            Delete,
        };
    }
    public static class ListingTemplates
    {
        public const string Insert = "Policy.ListingTemplate.Insert";
        public const string View = "Policy.ListingTemplate.View";
        public const string Update = "Policy.ListingTemplate.Update";
        public const string Delete = "Policy.ListingTemplate.Delete";
           
        public static readonly string[] All =
        {
            Insert,
            View,
            Update,
            Delete,
        };
    }
    public static class ProductListings
    {
        public const string Insert = "Policy.ProductListing.Insert";
        public const string View = "Policy.ProductListing.View";
        public const string Update = "Policy.ProductListing.Update";
        public const string Delete = "Policy.ProductListing.Delete";
           
        public static readonly string[] All =
        {
            Insert,
            View,
            Update,
            Delete,
        };
    }
    public static class ProductListingImages
    {
        public const string Insert = "Policy.ProductListingImage.Insert";
        public const string View = "Policy.ProductListingImage.View";
        public const string Update = "Policy.ProductListingImage.Update";
        public const string Delete = "Policy.ProductListingImage.Delete";
           
        public static readonly string[] All =
        {
            Insert,
            View,
            Update,
            Delete,
        };
    }
    public static class Orders
    {
        public const string Insert = "Policy.Order.Insert";
        public const string View = "Policy.Order.View";
        public const string Update = "Policy.Order.Update";
        public const string Delete = "Policy.Order.Delete";
           
        public static readonly string[] All =
        {
            Insert,
            View,
            Update,
            Delete,
        };
    }
    public static class Notifications
    {
        public const string View = "Policy.Notifications.View";
        public const string Delete = "Policy.Notifications.Delete";
           
        public static readonly string[] All =
        {
            View,
            Delete,
        };
    }
    public static string[] BuildAllPolicies()
    {
        var list = new List<string>();
        list.AddRange(Catalogs.All);
        list.AddRange(CatalogProducts.All);
        list.AddRange(CatalogProductImages.All);
        list.AddRange(InventoryProducts.All);
        list.AddRange(ListingTemplates.All);
        list.AddRange(InventoryProductImages.All);
        list.AddRange(ProductListings.All);
        list.AddRange(ProductListingImages.All);
        list.AddRange(Orders.All);
        list.AddRange(Notifications.All);
        return list.ToArray();
    }
    public static string[] BuildAllPermissions()
    {
        return BuildAllPolicies().Select(p => p.Replace("Policy", "Permission")).ToArray();
    }
}