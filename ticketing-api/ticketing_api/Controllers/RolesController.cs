using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sieve.Models;
using Sieve.Services;
using ticketing_api.Data;
using ticketing_api.Infrastructure;
using ticketing_api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace ticketing_api.Controllers
{
    /// <summary>
    /// Roles management as far as permissions
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : BaseController.BaseController
    {
        private readonly ILogger<RolesController> _logger;

        public RolesController(ApplicationDbContext context, ILogger<RolesController> logger, IEmailSender emailSender, ISieveProcessor sieveProcessor)
            : base(context, emailSender, sieveProcessor)
        {
            _logger = logger;
        }

        // GET: api/Roles
        [HttpGet]
        public async Task<IActionResult> GetRoleAsync([FromQuery]SieveModel sieveModel)
        {
            var isPermisssion = await CheckPermissions("ROLES", "Read");

            if (!isPermisssion)
            {
                return BadRequest("Read permission not allowed");
            }

            var roles = _context.AspNetRoles.AsQueryable();
            roles = _sieveProcessor.Apply(sieveModel, roles);
            return Ok(roles);
        }

        // GET: api/Roles/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetRole([FromRoute] int id)
        {
            var isPermisssion = await CheckPermissions("ROLES", "Read");

            if (!isPermisssion)
            {
                return BadRequest("Read permission not allowed");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var roles = await _context.Roles.FindAsync(id);

            if (roles == null)
            {
                return NotFound("Role Id not found");
            }

            return Ok(roles);
        }

        // POST: api/Roles
        [HttpPost]
        public async Task<IActionResult> PostRoleAsync([FromBody] IdentityRole role)
        {
            var isPermisssion = await CheckPermissions("ROLES", "Create");

            if (!isPermisssion)
            {
                return BadRequest("Create permission not allowed");
            }
 
            //set normalizedName in role
            role.Name = role.Name.ToUpper();
            role.NormalizedName = role.Name.ToUpper();
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var exists = await _context.AspNetRoles.AnyAsync(x => x.Name.Equals(role.Name,StringComparison.OrdinalIgnoreCase));
            if (exists)
            {
                return BadRequest($"{role.Name} already exists.");
            }
            _context.AspNetRoles.Add(role);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRole", new { id = role.Id }, role);
        }

        // PUT: api/Roles/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRoleAsync([FromRoute] string id, [FromBody] IdentityRole role)
        {
            var isPermisssion = await CheckPermissions("ROLES", "Update");

            if (!isPermisssion)
            {
                return BadRequest("Update permission not allowed");
            } 

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != role.Id)
            {
                return BadRequest("Requested role id does not match with queryString id");
            }

            //set normalized name in role
            role.Name = role.Name.ToUpper();
            role.NormalizedName = role.Name.ToUpper();

            var exists = await _context.AspNetRoles.AnyAsync(x => x.Id != role.Id && x.Name.Equals(role.Name, StringComparison.OrdinalIgnoreCase));
            if (exists)
            {
                return BadRequest($"{role.Name} already exists.");
            }

            try
            {
                _context.Entry(role).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RoleExists(id))
                {
                    return NotFound("Role Id not found");
                }
                throw;
            }

            return Ok(role);
        }

        // DELETE: api/Roles/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRoleAsync([FromRoute] string id)
        {
            var isPermisssion = await CheckPermissions("ROLES", "Delete");

            if (!isPermisssion)
            {
                return BadRequest("Delete permission not allowed");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var roles = await _context.Roles.FindAsync(id);
            if (roles == null)
            {
                return NotFound("Role Id not found");
            }

            // check the Role Id exist in Permission
            var roleExistPermission = _context.Permission.Count(p => p.RoleId == id);

            if (roleExistPermission > 0)
            {
                return BadRequest("Not able to delete role as it has reference in permission table");
            }

                _context.Roles.Remove(roles);
                await _context.SaveChangesAsync();

            return Ok(roles);
        }

        // Check Role Exist or Not
        private bool RoleExists(string id)
        {
            return _context.Roles.Any(e => e.Id == id);
        }
    }
}
