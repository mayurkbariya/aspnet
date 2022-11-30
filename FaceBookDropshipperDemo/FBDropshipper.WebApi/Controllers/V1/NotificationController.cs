using FBDropshipper.Application.Notifications.Commands.DeleteNotification;
using FBDropshipper.Application.Notifications.Commands.ReadAllNotification;
using FBDropshipper.Application.Notifications.Commands.ReadNotification;
using FBDropshipper.Application.Notifications.Queries.GetNotifications;
using FBDropshipper.Domain.Constant;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FBDropshipper.WebApi.Controllers.V1;

public class NotificationController : BaseController
{
    /// <summary>
    /// Get Notifications
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [Authorize(Policy = AppPolicy.Notifications.View)]
    [HttpGet]
    public async Task<GetNotificationsResponseModel> GetNotifications([FromQuery] GetNotificationsRequestModel model)
    {
        return await Mediator.Send(model);
    }

    /// <summary>
    /// Read Notification By Id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [Authorize(Policy = AppPolicy.Notifications.View)]
    [HttpPut("read/{id:int}")]
    public async Task<ReadNotificationResponseModel> ReadNotification([FromRoute] int id)
    {
        var model = new ReadNotificationRequestModel
        {
            Id = id
        };
        return await Mediator.Send(model);
    }

    /// <summary>
    /// Read All Notifications
    /// </summary>
    /// <returns></returns>
    [Authorize(Policy = AppPolicy.Notifications.View)]
    [HttpPut("read")]
    public async Task<ReadAllNotificationResponseModel> ReadAllNotification()
    {
        var model = new ReadAllNotificationRequestModel();
        return await Mediator.Send(model);
    }

    /// <summary>
    /// Delete Notification
    /// </summary>
    /// <returns></returns>
    [Authorize(Policy = AppPolicy.Notifications.Delete)]
    [HttpDelete("{id:int}")]
    public async Task<DeleteNotificationResponseModel> DeleteNotification([FromRoute] int id)
    {
        var model = new DeleteNotificationRequestModel()
        {
            Id = id
        };
        return await Mediator.Send(model);
    }
}