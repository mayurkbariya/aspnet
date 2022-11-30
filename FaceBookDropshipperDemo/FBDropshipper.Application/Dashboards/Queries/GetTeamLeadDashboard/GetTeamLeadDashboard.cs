using FBDropshipper.Application.Dashboards.Models;
using FBDropshipper.Application.Interfaces;
using FBDropshipper.Persistence.Context;
using FBDropshipper.Persistence.Extension;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FBDropshipper.Application.Dashboards.Queries.GetTeamLeadDashboard;

public class GetTeamLeadDashboardRequestModel : IRequest<GetTeamLeadDashboardResponseModel>
{
}

public class GetTeamLeadDashboardRequestModelValidator : AbstractValidator<GetTeamLeadDashboardRequestModel>
{
    public GetTeamLeadDashboardRequestModelValidator()
    {
        
    }
}

public class
    GetTeamLeadDashboardRequestHandler : IRequestHandler<GetTeamLeadDashboardRequestModel,
        GetTeamLeadDashboardResponseModel>
{
    private readonly ApplicationDbContext _context;
    private readonly ISessionService _sessionService;
    public GetTeamLeadDashboardRequestHandler(ApplicationDbContext context, ISessionService sessionService)
    {
        _context = context;
        _sessionService = sessionService;
    }

    public async Task<GetTeamLeadDashboardResponseModel> Handle(GetTeamLeadDashboardRequestModel request,
        CancellationToken cancellationToken)
    {
        var userId = _sessionService.GetTeamLeaderIdOrUserId();
        var marketplace = await _context.MarketPlaces
            .GetAllReadOnly(p => p.Team.UserId == userId)
            .Select(DashboardSelector.TeamLeadSelector)
            .ToListAsync(cancellationToken: cancellationToken);
        return new GetTeamLeadDashboardResponseModel()
        {
            Data = marketplace
        };
    }

}

public class GetTeamLeadDashboardResponseModel
{
    public List<TeamLeadDashboardDto> Data { get; set; }
}