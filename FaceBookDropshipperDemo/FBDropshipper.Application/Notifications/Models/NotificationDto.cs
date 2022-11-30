using System.Linq.Expressions;
using FBDropshipper.Common.Extensions;
using FBDropshipper.Domain.Entities;

namespace FBDropshipper.Application.Notifications.Models
{
    public class NotificationDto
    {
        public NotificationDto()
        {
        }

        public NotificationDto(UserNotification notification)
        {
            Id = notification.Id;
            Message = notification.Message;
            IsRead = notification.IsRead;
            CreatedDate = notification.CreatedDate.ToGeneralDateTime();
        }

        public int Id { get; set; }
        public string Message { get; set; }
        public bool IsRead { get; set; }
        public string CreatedDate { get; set; }
        public object Data { get; set; }
        public int Type { get; set; }
    }

    public class NotificationSelector
    {
        public static Expression<Func<UserNotification, NotificationDto>> Selector = p => new NotificationDto()
        {
            Id = p.Id,
            Message = p.Message,
            Data = p.Data,
            CreatedDate = p.CreatedDate.ToGeneralDateTime(),
            IsRead = p.IsRead,
            Type = p.Type
        };
    }
}