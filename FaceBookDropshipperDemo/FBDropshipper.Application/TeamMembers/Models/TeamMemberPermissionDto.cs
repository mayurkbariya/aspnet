using System.Linq.Expressions;
using FBDropshipper.Domain.Entities;

namespace FBDropshipper.Application.TeamMembers.Models;

public class TeamMemberPermissionDto
{
    public int[] MarketPlaceIds { get; set; }
    public string[] Permissions { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
}

public class PermissionSelector
{
    public static Expression<Func<User, TeamMemberPermissionDto>> Selector = p => new TeamMemberPermissionDto()
    {
        FullName = p.FullName,
        Email = p.Email,
        Permissions = p.UserClaims.Select(pr => pr.ClaimValue).ToArray(),
        MarketPlaceIds = p.TeamMemberPermissions.Select(pr => pr.MarketPlaceId).ToArray()
    };
}