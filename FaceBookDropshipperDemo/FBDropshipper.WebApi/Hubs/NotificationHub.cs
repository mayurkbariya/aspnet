using FBDropshipper.Domain.Constant;
using FBDropshipper.WebApi.Extension;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace FBDropshipper.WebApi.Hubs
{
    [Authorize]
    public class NotificationHub : Hub
    {
        public override Task OnConnectedAsync()
        {
            if (Context.User != null && Context.User.IsInRole(RoleNames.Admin))
            {
                Groups.AddToGroupAsync(Context.ConnectionId, RoleNames.Admin, Context.ConnectionAborted);
            }
            Groups.AddToGroupAsync(Context.ConnectionId, Context.User.GetUserId(), Context.ConnectionAborted);
            return base.OnConnectedAsync();
        }
    }
}