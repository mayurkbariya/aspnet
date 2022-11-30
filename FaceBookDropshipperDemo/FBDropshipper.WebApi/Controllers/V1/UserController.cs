using FBDropshipper.Application.Users.Commands.ChangePassword;
using FBDropshipper.Application.Users.Commands.DisableUser;
using FBDropshipper.Application.Users.Commands.EnableUser;
using FBDropshipper.Application.Users.Commands.ForgetPassword;
using FBDropshipper.Application.Users.Commands.RegisterUser;
using FBDropshipper.Application.Users.Commands.ResetPassword;
using FBDropshipper.Application.Users.Commands.VerifyUser;
using FBDropshipper.Application.Users.Queries.GetUsers;
using FBDropshipper.Domain.Constant;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FBDropshipper.WebApi.Controllers.V1
{
    public class UserController : BaseController
    {
        
        /// <summary>
        /// Get Users
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize(Roles = RoleNames.Admin)]
        [HttpGet]
        public async Task<GetUsersResponseModel> GetUsers([FromQuery] GetUsersRequestModel model)
        {
            return await Mediator.Send(model);
        }
        
        
        /// <summary>
        /// Enable User
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = RoleNames.Admin)]
        [HttpPut("enable/{id}")]
        public async Task<EnableUserResponseModel> EnableUser([FromRoute] string id)
        {
            return await Mediator.Send(new EnableUserRequestModel()
            {
                Id = id
            });
        }
        
        /// <summary>
        /// Disable User
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = RoleNames.Admin)]
        [HttpPut("disable/{id}")]
        public async Task<DisableUserResponseModel> DisableUser([FromRoute] string id)
        {
            return await Mediator.Send(new DisableUserRequestModel()
            {
                Id = id
            });
        }
        
        
        
        /// <summary>
        /// Register User
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("register")]
        public async Task<RegisterUserResponseModel> RegisterUser([FromBody] RegisterUserRequestModel model)
        {
            return await Mediator.Send(model);
        }
        
        /// <summary>
        /// Verify User
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("verify")]
        public async Task<VerifyUserResponseModel> VerifyUser([FromBody] VerifyUserRequestModel model)
        {
            return await Mediator.Send(model);
        }
        
        /// <summary>
        /// Forget Password
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut("password/forget")]
        public async Task<ForgetPasswordResponseModel> ForgetPassword([FromBody] ForgetPasswordRequestModel model)
        {
            return await Mediator.Send(model);
        }
        
        /// <summary>
        /// Reset Password
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut("password/reset")]
        public async Task<ResetPasswordResponseModel> ResetPassword([FromBody] ResetPasswordRequestModel model)
        {
            return await Mediator.Send(model);
        }
        
        /// <summary>
        /// Change Password of Current User
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPut("password")]
        public async Task<ChangePasswordResponseModel> ChangePassword([FromBody] ChangePasswordRequestModel model)
        {
            return await Mediator.Send(model);
        }
    }
}