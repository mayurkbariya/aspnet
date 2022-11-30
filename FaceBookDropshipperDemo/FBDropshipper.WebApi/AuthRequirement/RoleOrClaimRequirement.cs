using System.Security.Claims;
using FBDropshipper.WebApi.Extension;
using Microsoft.AspNetCore.Authorization;

namespace FBDropshipper.WebApi.AuthRequirement;

public class RoleOrClaimRequirement : IAuthorizationRequirement
{
    public RoleOrClaimRequirement(string role, Claim claim)
    {
        Role = role;
        Claim = claim;
    }

    public string Role { get; }
    public Claim Claim { get; }
}

public class RoleOrClaimRequirementHandler : AuthorizationHandler<RoleOrClaimRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
        RoleOrClaimRequirement requirement)
    {
        if (context.User.GetRole() == requirement.Role)
        {
            context.Succeed(requirement);
            return Task.CompletedTask;
        }

        if (context.User.HasClaim(requirement.Claim.Type,requirement.Claim.Value))
        {
            context.Succeed(requirement);
            return Task.CompletedTask;
        }
        context.Fail();
        return Task.CompletedTask;
    }
}