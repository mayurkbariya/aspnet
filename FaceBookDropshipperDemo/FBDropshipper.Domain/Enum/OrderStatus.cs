namespace FBDropshipper.Domain.Enum;

public enum OrderStatus
{
    ToBeShipped = 0,
    TrackingUploaded = 1,
    Refund = 2,
    Cancelled = 3,
    Archived = 4
}