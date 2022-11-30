using System.Threading.Tasks;

namespace FBDropshipper.Application.Interfaces
{
    public interface INotificationService
    {
        Task<bool> SendNotification(string fcmId, string title, string message);
        Task<bool> SendNotification(string fcmId, string title, string body, object data);
        Task<bool> SendNotificationData(string fcmId, object data);
    }
}