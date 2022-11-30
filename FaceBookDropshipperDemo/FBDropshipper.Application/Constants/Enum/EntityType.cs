namespace FBDropshipper.Application.Constants.Enum
{
    public enum EntityType
    {
        Category,
        SubCategory,
        Product,
        Customer,
        Supplier,
        Sale,
        Purchase,
        SaleReturn,
        PurchaseReturn
    }

    public enum InvoiceStatus
    {
        Pending,
        Partial,
        Paid
    }
}