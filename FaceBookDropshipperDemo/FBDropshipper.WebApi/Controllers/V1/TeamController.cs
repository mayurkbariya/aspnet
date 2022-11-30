using System.Threading.Tasks;
using FBDropshipper.Application.Teams.Commands.UpdateTeam;
using FBDropshipper.Application.Teams.Queries.GetTeams;
using FBDropshipper.Domain.Constant;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FBDropshipper.WebApi.Controllers.V1
{
    public class TeamController : BaseController
    {
        /// <summary>
        /// Get Teams
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize(Roles = RoleNames.TeamLeader)]
        [HttpGet]
        public async Task<GetTeamsResponseModel> GetTeams([FromQuery] GetTeamsRequestModel model)
        {
            return await Mediator.Send(model);
        }
        
        /// <summary>
        /// Update Team
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize(Roles = RoleNames.TeamLeader)]
        [HttpPut("{id:int}")]
        public async Task<UpdateTeamResponseModel> CreateTeamMember([FromRoute] int id, UpdateTeamRequestModel model)
        {
            model.Id = id;
            return await Mediator.Send(model);
        }
    }
}