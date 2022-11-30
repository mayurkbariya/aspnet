using FBDropshipper.Application.Dashboards.Queries.GetTeamLeadDashboard;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FBDropshipper.WebApi.Controllers.V1;

public class DashboardController : BaseController
{
    [Authorize]
    [HttpGet("teamLead")]
    public async Task<GetTeamLeadDashboardResponseModel> GetTeamLoadDashboard()
    {
        return await Mediator.Send(new GetTeamLeadDashboardRequestModel());
    }
}