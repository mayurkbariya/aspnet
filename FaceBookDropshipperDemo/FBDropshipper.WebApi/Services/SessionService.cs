using System;
using FBDropshipper.Application.Interfaces;
using FBDropshipper.Common.Extensions;
using FBDropshipper.Domain.Constant;
using FBDropshipper.WebApi.Extension;
using Microsoft.AspNetCore.Http;

namespace FBDropshipper.WebApi.Services
{
    public class SessionService : ISessionService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SessionService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetTimeZone()
        {
            var data = _httpContextAccessor.HttpContext?.Request.Headers["TimeZone"];
            if (!data.HasValue)
            {
                return "";
            }
            return data.ToString();
        }
        
        public string GetUserId()
        {
            CheckContext();
            return _httpContextAccessor.HttpContext?.User.GetUserId();
        }
        

        public string GetRole()
        {
            CheckContext();
            return _httpContextAccessor.HttpContext?.User.GetRole();
        }

        public string GetTeamLeaderId()
        {
            return _httpContextAccessor.HttpContext?.User.GetTeamLeaderId();
        }

        public string GetTeamLeaderIdOrUserId()
        {
            if (GetRole() == RoleNames.TeamMember)
            {
                return GetTeamLeaderId();
            }
            return GetUserId();
        }

        public bool IsAdmin()
        {
            return GetRole() == RoleNames.Admin;
        }

        public string[] GetAllUserIds()
        {
            var userId = GetUserId();
            var teamMemberId = GetTeamLeaderId();
            var list = new List<string>();
            if (userId.IsNotNullOrWhiteSpace())
            {
                list.Add(userId);
            }
            if (teamMemberId.IsNotNullOrWhiteSpace())
            {
                list.Add(teamMemberId);
            }
            return list.ToArray();
        }

        private void CheckContext()
        {
            if (_httpContextAccessor.HttpContext == null || _httpContextAccessor.HttpContext.User.GetUserId().IsNullOrWhiteSpace())
            {
                throw new UnauthorizedAccessException();
            }
        }
        
    }
}