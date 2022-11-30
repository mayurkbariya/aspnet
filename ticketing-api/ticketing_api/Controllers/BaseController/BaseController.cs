using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sieve.Services;
using ticketing_api.Data;
using ticketing_api.Infrastructure;
using ticketing_api.Models;
using ticketing_api.Services;

namespace ticketing_api.Controllers.BaseController
{
    
    public class BaseController : ControllerBase
    {
        protected readonly ApplicationDbContext _context;
        protected readonly IEmailSender _emailSender;
        protected readonly ApplicationSieveProcessor _sieveProcessor;

        public BaseController(ApplicationDbContext context, IEmailSender emailSender, ISieveProcessor sieveProcessor)
        {
            _context = context;
            _emailSender = emailSender;
            _sieveProcessor = (ApplicationSieveProcessor)sieveProcessor;
        }

        private AppUser _appUser;
        protected AppUser AppUser
        {
            get
            {
                if (_appUser == null)
                {
                    var adminId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                    _appUser = _context.AppUser.FirstOrDefault(x => x.AspNetUserId == adminId);
                }

                return _appUser;
            }
        }

        protected  async Task<bool> CheckPermissions(string module, string permission)
        {
            var roleName = User.FindFirst(ClaimTypes.Role).Value;
            var roleId = await _context.Roles.FirstAsync(x => x.Name == roleName);
            var permissions = await _context.Permission
                .Join(_context.Module, a => a.ModuleId, b => b.Id, (a, b) => new
                {
                    a.Id,
                    a.RoleId,
                    a.ModuleId,
                    b.ModuleName,
                    a.IsRead,
                    a.IsCreate,
                    a.IsUpdate,
                    a.IsDelete
                }).Where(x => x.RoleId == roleId.Id).ToListAsync();

            foreach (var p in permissions)
            {
                if (p.ModuleName.ToUpper() == module.ToUpper())
                {
                    switch (permission)
                    {
                        case "Create":
                            return p.IsCreate;
                        case "Update":
                            return p.IsUpdate;
                        case "Read":
                            return p.IsRead;
                        case "Delete":
                            return p.IsDelete;
                    }
                }
            }

            return false;
        }
    }
}
