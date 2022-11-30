using System.Security.Claims;
using FBDropshipper.Common.Constants;
using FBDropshipper.Domain.Constant;

namespace FBDropshipper.WebApi.Extension
{
    public static class ClaimPrincipleExtension
    {
        public static string GetUserId(this ClaimsPrincipal claimsPrincipal)
        {
            return claimsPrincipal.FindFirstValue(CustomClaimTypes.UserId);
        }

        public static int[] GetMarketplaces(this ClaimsPrincipal claimsPrincipal)
        {
            var marketplaces = claimsPrincipal.FindAll(p => p.Type == CustomClaimTypes.MarketPlace).ToList();
            return marketplaces.Any() ? marketplaces.Select(p => int.Parse(p.Value)).ToArray() : Array.Empty<int>();
        }
        public static string GetMerchantId(this ClaimsPrincipal claimsPrincipal)
        {
            return claimsPrincipal.FindFirstValue(CustomClaimTypes.MerchantId);
        }
        
        public static int GetCompanyId(this ClaimsPrincipal claimsPrincipal)
        {
            var id = claimsPrincipal.FindFirstValue(CustomClaimTypes.CompanyId);
            int.TryParse(id, out var companyId);
            return companyId;
        }

        public static string GetTeamLeaderId(this ClaimsPrincipal claimsPrincipal)
        {
            var id = claimsPrincipal.FindFirstValue(CustomClaimTypes.TeamLeaderId);
            return id;
        }
        public static string GetRole(this ClaimsPrincipal claimsPrincipal)
        {
            if (claimsPrincipal.IsInRole(RoleNames.Admin))
            {
                return RoleNames.Admin;
            }
            if (claimsPrincipal.IsInRole(RoleNames.TeamLeader))
            {
                return RoleNames.TeamLeader;
            }
            if (claimsPrincipal.IsInRole(RoleNames.TeamMember))
            {
                return RoleNames.TeamMember;
            }
            return "";
        }
    }
}