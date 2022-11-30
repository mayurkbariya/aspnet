using FBDropshipper.Application.Interfaces;
using FBDropshipper.Common.Extensions;
using FBDropshipper.Domain.Enum;
using FBDropshipper.WebApi.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace FBDropshipper.WebApi.Services
{
    public class AlertService : IAlertService
    {
        private readonly IHubContext<NotificationHub> _hub;

        public AlertService(IHubContext<NotificationHub> hub)
        {
            _hub = hub;
        }

        public async Task<bool> SendNotificationToUser(string userId, string message, NotificationType type, object data = null)
        {
            await _hub.Clients.Group(userId).SendCoreAsync("onNewNotification",new object[]{ message, type, data});
            return true;
        }
    }
}