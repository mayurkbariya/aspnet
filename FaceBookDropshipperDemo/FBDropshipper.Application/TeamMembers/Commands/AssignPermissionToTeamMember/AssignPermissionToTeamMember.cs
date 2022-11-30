using FBDropshipper.Application.Exceptions;
using FBDropshipper.Application.Extensions;
using FBDropshipper.Application.Interfaces;
using FBDropshipper.Common.Constants;
using FBDropshipper.Domain.Constant;
using FBDropshipper.Domain.Entities;
using FBDropshipper.Persistence.Context;
using FBDropshipper.Persistence.Extension;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FBDropshipper.Application.TeamMembers.Commands.AssignPermissionToTeamMember;

public class AssignPermissionToTeamMemberRequestModel : IRequest<AssignPermissionToTeamMemberResponseModel>
{
    public string TeamMemberId { get; set; }
    public int[] MarketPlaceIds { get; set; }
    public List<string> Permissions { get; set; }
}

public class
    AssignPermissionToTeamMemberRequestModelValidator : AbstractValidator<AssignPermissionToTeamMemberRequestModel>
{
    public AssignPermissionToTeamMemberRequestModelValidator()
    {
        RuleFor(p => p.TeamMemberId).Required();
        RuleFor(p => p.MarketPlaceIds).Required();
        RuleForEach(p => p.Permissions).MustBeOneOf(AppPolicy.BuildAllPermissions());
    }
}

public class
    AssignPermissionToTeamMemberRequestHandler : IRequestHandler<AssignPermissionToTeamMemberRequestModel,
        AssignPermissionToTeamMemberResponseModel>
{
    private readonly ApplicationDbContext _context;
    private readonly ISessionService _sessionService;
    public AssignPermissionToTeamMemberRequestHandler(ApplicationDbContext context, ISessionService sessionService)
    {
        _context = context;
        _sessionService = sessionService;
    }

    public async Task<AssignPermissionToTeamMemberResponseModel> Handle(
        AssignPermissionToTeamMemberRequestModel request,
        CancellationToken cancellationToken)
    {
        var userId = _sessionService.GetUserId();
        var user = await _context.Users
            .GetByAsync(p => p.TeamMember.Team.UserId == userId, 
            p => 
                p.Include(pr => pr.TeamMemberPermissions)
                .Include(pr => pr.UserClaims),
            cancellationToken: cancellationToken);
        if (user == null)
        {
            throw new NotFoundException(nameof(user));
        }
        user.UserClaims.Clear();
        user.TeamMemberPermissions.Clear();
        request.MarketPlaceIds.Select(p => new TeamMemberPermission()
        {
            MarketPlaceId = p
        }).ToList().ForEach(p =>
        {
            user.TeamMemberPermissions.Add(p);
        });
        request.Permissions.Select(p => new UserClaim()
        {
            ClaimType = CustomClaimTypes.Permission,
            ClaimValue = p
        }).ToList().ForEach(p =>
        {
            user.UserClaims.Add(p);
        });
        _context.Users.Update(user);
        await _context.SaveChangesAsync(cancellationToken);
        return new AssignPermissionToTeamMemberResponseModel();
    }
    

}

public class AssignPermissionToTeamMemberResponseModel
{

}