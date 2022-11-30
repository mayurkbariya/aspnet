using System.Threading.Tasks;
using FBDropshipper.Application.TeamMembers.Commands.AssignPermissionToTeamMember;
using FBDropshipper.Application.TeamMembers.Commands.CreateTeamMember;
using FBDropshipper.Application.TeamMembers.Commands.DeleteTeamMember;
using FBDropshipper.Application.TeamMembers.Queries.GetPermissionByTeamMember;
using FBDropshipper.Application.TeamMembers.Queries.GetPermissions;
using FBDropshipper.Application.TeamMembers.Queries.GetTeamMembersByTeamId;
using FBDropshipper.Domain.Constant;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FBDropshipper.WebApi.Controllers.V1
{
    public class TeamMemberController : BaseController
    {
        /// <summary>
        /// Create Team Member
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize(Roles = RoleNames.TeamLeader)]
        [HttpPost]
        public async Task<CreateTeamMemberResponseModel> CreateTeamMember(CreateTeamMemberRequestModel model)
        {
            return await Mediator.Send(model);
        }
        
        
        /// <summary>
        /// Get Permissions
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = RoleNames.TeamLeader)]
        [HttpGet("permission")]
        public async Task<GetPermissionsResponseModel> GetPermission()
        {
            return await Mediator.Send(new GetPermissionsRequestModel());
        }
        
        
        /// <summary>
        /// Assign Permissions with Marketplace
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = RoleNames.TeamLeader)]
        [HttpPut("permission/{id}")]
        public async Task<AssignPermissionToTeamMemberResponseModel> AssignPermissionToTeamMember([FromRoute] string id,AssignPermissionToTeamMemberRequestModel model)
        {
            model.TeamMemberId = id;
            return await Mediator.Send(model);
        }
        
        /// <summary>
        /// Get Permissions By Id
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = RoleNames.TeamLeader)]
        [HttpGet("permission/{id}")]
        public async Task<GetPermissionByTeamMemberResponseModel> GetPermissionById([FromRoute] string id)
        {
            var model = new GetPermissionByTeamMemberRequestModel
            {
                Id = id
            };
            return await Mediator.Send(model);
        }
        
        /// <summary>
        /// Delete Team Member
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = RoleNames.TeamLeader)]
        [HttpDelete("{id}")]
        public async Task<DeleteTeamMemberResponseModel> DeleteTeamMember([FromRoute] string id)
        {
            var model = new DeleteTeamMemberRequestModel()
            {
                Id = id
            };
            return await Mediator.Send(model);
        }
        
        /// <summary>
        /// Get Team Members By TeamId
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize(Roles = RoleNames.TeamLeader)]
        [HttpGet]
        public async Task<GetTeamMembersByTeamIdResponseModel> GetTeamMembers([FromQuery] GetTeamMembersByTeamIdRequestModel model)
        {
            return await Mediator.Send(model);
        }
    }
}