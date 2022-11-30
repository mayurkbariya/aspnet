namespace FBDropshipper.Application.Constants.Enum
{
    public enum OrderType
    {
        All = -1,
        Sale = 0,
        Purchase,
        Manual,
        SaleReturn,
        PurchaseReturn
    }

    public enum PayerType
    {
        All = -1,
        Customer = 0,
        Supplier,
    }

    public enum PaymentMethodType
    {
        All = -1,
        Cash = 0,
        Debit,
        Cheque
    }
}