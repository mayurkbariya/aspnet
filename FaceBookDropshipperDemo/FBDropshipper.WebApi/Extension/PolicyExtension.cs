using System.Security.Claims;
using FBDropshipper.Common.Constants;
using FBDropshipper.Domain.Constant;
using FBDropshipper.WebApi.AuthRequirement;
using Microsoft.AspNetCore.Authorization;

namespace FBDropshipper.WebApi.Extension;

public static class PolicyExtension
{
    public static void AddAllPolicies(this AuthorizationOptions options)
    {

        var policies = AppPolicy.BuildAllPolicies();
        for (int i = 0; i < policies.Length; i++)
        {
            var permission = policies[i].Replace("Policy","Permission");
            options.AddPolicy(policies[i], p => p
                .AddRequirements(new RoleOrClaimRequirement(RoleNames.TeamLeader,
                    new Claim(CustomClaimTypes.Permission, permission))));
        }
    }
}