using FBDropshipper.Domain.Enum;

namespace FBDropshipper.Domain.Entities;

public class UserNotification : Base
{
    public string UserId { get; set; }
    public User User { get; set; }
    /// <summary>
    /// <see cref="NotificationType"/>
    /// </summary>
    public int Type { get; set; }
    public bool IsRead { get; set; }
    public DateTime? ReadDate { get; set; }
    public string Message { get; set; }
    public string Data { get; set; }
}