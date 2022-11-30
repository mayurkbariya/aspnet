using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sieve.Models;
using Sieve.Services;
using ticketing_api.Data;
using ticketing_api.Infrastructure;
using ticketing_api.Models;

namespace ticketing_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SettingsController : BaseController.BaseController
    {
        private readonly ILogger<SettingsController> _logger;       

        public SettingsController(ApplicationDbContext context, ILogger<SettingsController> logger, IEmailSender emailSender, ISieveProcessor sieveProcessor) : base(context, emailSender, sieveProcessor)
        {           
            _logger = logger;
        }

        // GET: api/Settings
        [HttpGet]
        public async Task<IActionResult> GetSetting([FromQuery]SieveModel sieveModel)
        {
            var isPermisssion = await CheckPermissions("SETTINGS", "Read");

            if (!isPermisssion)
            {
                return BadRequest("Read permission not allowed");
            }

            var query = _context.Setting.AsQueryable();
            var data = await _sieveProcessor.GetPagingDataAsync(sieveModel, query);
            return Ok(data);
        }

        // GET: api/Settings/4
        [HttpGet("{id}")]
        public async Task<IActionResult> GetSetting([FromRoute] int id)
        {
            var isPermisssion = await CheckPermissions("SETTINGS", "Read");

            if (!isPermisssion)
            {
                return BadRequest("Read permission not allowed");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var setting = await _context.Setting.FindAsync(id);

            if (setting == null)
            {
                return NotFound("Setting Id not found");
            }

            return Ok(setting);
        }

        // GET: api/Settings
        [HttpGet("GetSettingKey")]
        public async Task<IActionResult> GetSettingKey([FromQuery] string key)
        {
            var isPermisssion = await CheckPermissions("SETTINGS", "Read");

            if (!isPermisssion)
            {
                return BadRequest("Filter Key permission not allowed");
            }

            var settingKey = _context.Setting.FirstOrDefault(s => s.Key == key);
            return Ok(settingKey?.Value);
        }

        // POST : api/Settings
        [HttpPost]
        public async Task<IActionResult> PostSetting([FromBody] Setting setting)
        {
            var isPermisssion = await CheckPermissions("SETTINGS", "Create");

            if (!isPermisssion)
            {
                return BadRequest("Create permission not allowed");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var settingNameExists = _context.Setting.FirstOrDefault(r => r.Key.Equals(setting.Key, StringComparison.OrdinalIgnoreCase));
            if (settingNameExists != null)
            {
                return BadRequest("Setting key already exists");
            }
            else
            {
                _context.Setting.Add(setting);
                await _context.SaveChangesAsync();
            }

            return CreatedAtAction("GetSetting", new { id = setting.Id }, setting);
        }

        // PUT: api/Settings/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSetting([FromRoute] int id, [FromBody] Setting setting)
        {
            var isPermisssion = await CheckPermissions("SETTINGS", "Update");

            if (!isPermisssion)
            {
                return BadRequest("Update permission not allowed");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != setting.Id)
            {
                return BadRequest("Requested setting id does not match with queryString id");
            }           

            try
            {
                var settingKey = _context.Setting.Where(s => s.Id == setting.Id).Select(s => s.Key).Single();
                
                if (settingKey == setting.Key)
                {
                    _context.Entry(setting).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                }
                else
                {
                    var settingNameExists = _context.Setting.Count(s => s.Id != setting.Id && s.Key.Equals(setting.Key, StringComparison.OrdinalIgnoreCase));
                    if (settingNameExists > 0)
                    {
                        return BadRequest("Setting key already exists");
                    }
                    else
                    {
                        _context.Entry(setting).State = EntityState.Modified;
                        await _context.SaveChangesAsync();
                    }
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SettingExists(id))
                {
                    return NotFound("Setting Id not found");
                }

                    throw;
                }
            return Ok(setting);
        }

        // DELETE: api/Settings/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSetting([FromRoute] int id)
        {
            var isPermisssion = await CheckPermissions("SETTINGS", "Delete");

            if (!isPermisssion)
            {
                return BadRequest("Delete permission not allowed");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var setting = await _context.Setting.FindAsync(id);
            if (setting == null)
            {
                return NotFound("Setting Id not found");
            }

            _context.Setting.Remove(setting);
            await _context.SaveChangesAsync();

            return Ok(setting);
        }

        // Check if Setting Exists or Not
        private bool SettingExists(int id)
        {
            return _context.Setting.Any(e => e.Id == id);
        }
    }
}
