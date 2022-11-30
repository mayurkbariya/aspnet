using System.Threading.Tasks;
using FBDropshipper.Application.Subscriptions.Commands.CancelSubscription;
using FBDropshipper.Application.Subscriptions.Commands.SubscribeToSubscription;
using FBDropshipper.Application.Subscriptions.Commands.UpdateSubscription;
using FBDropshipper.Application.Subscriptions.Queries.GetSubscriptions;
using FBDropshipper.Domain.Constant;
using FBDropshipper.WebApi.Extension;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FBDropshipper.WebApi.Controllers.V1
{
    public class SubscriptionController : BaseController
    {
        /// <summary>
        /// Cancel Subscription
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = RoleNames.TeamLeader)]
        [HttpDelete("cancel")]
        public async Task<CancelSubscriptionResponseModel> CancelSubscription()
        {
            return await Mediator.Send(new CancelSubscriptionRequestModel()
            {
                UserId = User.GetUserId()
            });
        }
        
        
        /// <summary>
        /// Subscribe To Subscription
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = RoleNames.TeamLeader)]
        [HttpPost("subscribe/{id:int}")]
        public async Task<SubscribeToSubscriptionResponseModel> SubscribeToSubscription([FromRoute] int id)
        {
            return await Mediator.Send(new SubscribeToSubscriptionRequestModel()
            {
                SubscriptionId = id
            });
        }
        
        /// <summary>
        /// Update Subscription
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize(Roles = RoleNames.Admin)]
        [HttpPut("{id:int}")]
        public async Task<UpdateSubscriptionResponseModel> UpdateSubscription([FromRoute] int id, UpdateSubscriptionRequestModel model)
        {
            model.Id = id;
            return await Mediator.Send(model);
        }
        
        /// <summary>
        /// Get Subscriptions
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize(Roles = RoleNames.Admin + "," + RoleNames.TeamLeader)]
        [HttpGet]
        public async Task<GetSubscriptionsResponseModel> GetSubscriptions([FromQuery] GetSubscriptionsRequestModel model)
        {
            return await Mediator.Send(model);
        }
        
    }
}