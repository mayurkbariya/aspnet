using System.Threading.Tasks;
using FBDropshipper.Domain.Enum;

namespace FBDropshipper.Application.Interfaces
{
    public interface IAlertService
    {
        Task<bool> SendNotificationToUser(string userId, string message, NotificationType type, object data = null);
    }
}