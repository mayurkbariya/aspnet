using System.Threading.Tasks;
using FBDropshipper.Application.UserSubscriptions.Queries.GetCurrentUserSubscription;
using FBDropshipper.Application.UserSubscriptions.Queries.GetUserSubscriptions;
using FBDropshipper.Domain.Constant;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FBDropshipper.WebApi.Controllers.V1
{
    public class UserSubscriptionController : BaseController
    {
        /// <summary>
        /// Get All User Subscriptions
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = RoleNames.TeamLeader)]
        [HttpGet]
        public async Task<GetUserSubscriptionsResponseModel> GetUserSubscription()
        {
            return await Mediator.Send(new GetUserSubscriptionsRequestModel());
        }
        
        /// <summary>
        /// Get Current Subscription
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = RoleNames.TeamLeader)]
        [HttpGet("current")]
        public async Task<GetCurrentUserSubscriptionResponseModel> GetCurrentUserSubscription()
        {
            return await Mediator.Send(new GetCurrentUserSubscriptionRequestModel());
        }
    }
}