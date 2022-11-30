using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sieve.Models;
using Sieve.Services;
using ticketing_api.Data;
using ticketing_api.Infrastructure;
using ticketing_api.Models;
using ticketing_api.Models.Views;
using ticketing_api.Services;

namespace ticketing_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WellsController : BaseController.BaseController
    {
        private readonly ILogger<WellsController> _logger;
        private readonly WellService _wellService;
        public SieveModel sieveModel;

        public WellsController(ApplicationDbContext context, ILogger<WellsController> logger, IEmailSender emailSender, ISieveProcessor sieveProcessor) : base(context, emailSender, sieveProcessor)
        {
            _logger = logger;
            _wellService = new WellService(_context, sieveProcessor);
        }
        // GET: api/Wells
        [HttpGet]
        public async Task<IActionResult> GetWellAsync([FromQuery] SieveModel sieveModel)
        {
            bool isPermisssion = await CheckPermissions("WELLS", "Read");

            if (!isPermisssion)
            {
                return BadRequest("Read permission not allowed");
            }

            PagingResults<WellView> wellView = await _wellService.GetWellViewAsync(sieveModel);
            return Ok(wellView);
        }

        // GET: api/Wells/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetWell([FromRoute] int id)
        {
            var isPermisssion = await CheckPermissions("WELLS", "Read");

            if (!isPermisssion)
            {
                return BadRequest("Read permission not allowed");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var well = await _context.Well.FindAsync(id);

            if (well == null)
            {
                return NotFound("Well Id not found");
            }

            return Ok(well);
        }

        /// <summary>
        /// Get well based on rig filter
        /// </summary>
        /// <param name="rigLocationId">rigLocation id</param>
        /// <returns></returns>
        //GET: api/Wells/1/filterwellbyrig
        [HttpGet("{rigLocationId}/filterwellbyrig")]
        public async Task<IActionResult> Filter([FromRoute] int rigLocationId)
        {
            var isPermisssion = await CheckPermissions("WELLS", "Read");

            if (!isPermisssion)
            {
                return BadRequest("Filter Permission not allowed");
            }

            var resultWell = _context.Well.Where(x => x.RigLocationId == rigLocationId).AsQueryable().OrderBy(well => well.Name);
            return Ok(resultWell);
        }

        // POST: api/Wells
        [HttpPost]
        public async Task<IActionResult> PostWellAsync([FromBody] Well well)
        {
            if (!ModelState.IsValid)
            {
                return NewMethod();
            }

            var isPermisssion = await CheckPermissions("WELLS", "Create");

            if (!isPermisssion)
            {
                return BadRequest("Create permission not allowed");
            }

            var wellNameExists = await _context.Well.FirstOrDefaultAsync(w => w.Name.Equals(well.Name, StringComparison.OrdinalIgnoreCase));
            if (wellNameExists != null)
            {
                return BadRequest("Well name already exists");
            }
            else
            {
                _context.Well.Add(well);
                 await _context.SaveChangesAsync();
            }

            WellView wellView = _wellService.PostWell(well);

            return CreatedAtAction("GetWell", new { id = well.Id }, wellView);
        }

        // PUT: api/Wells/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutWellAsync([FromRoute] int id, [FromBody] Well well)
        {
            var isPermisssion = await CheckPermissions("WELLS", "Update");

            if (!isPermisssion)
            {
                return BadRequest("Update permission not allowed");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != well.Id)
            {
                return BadRequest("Requested well id does not match with querystring id");
            }

            try
            {
                var wellName = await _context.Well.AsNoTracking().FirstOrDefaultAsync(w => w.Id == well.Id);

                if(wellName == null)
                {
                    return BadRequest("Well Id not found");
                }
                else if (wellName.Name == well.Name)
                {
                    _context.Entry(well).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                }
                else
                {
                    var wellNameExists = _context.Well.Count(w => w.Id != well.Id && w.Name.Equals(well.Name, StringComparison.OrdinalIgnoreCase));
                    if (wellNameExists > 0)
                    {
                        return BadRequest("Well name already exists");
                    }
                    else
                    {
                        _context.Entry(well).State = EntityState.Modified;
                        await _context.SaveChangesAsync();
                    }
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WellExists(id))
                {
                    return NotFound("Well Id not found");
                }
                else
                {
                    throw;
                }
            }

            WellView wellView = _wellService.PostWell(well);

            return Ok(wellView);
        }

        private IActionResult NewMethod()
        {
            return BadRequest(ModelState);
        }

        // DELETE: api/Wells/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWellAsync([FromRoute] int id)
        {
            var isPermisssion = await CheckPermissions("WELLS", "Delete");

            if (!isPermisssion)
            {
                return BadRequest("Delete permission not allowed");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var well = await _context.Well.FindAsync(id);
            if (well == null)
            {
                return NotFound("Well Id not found");
            }

            // check the Well Id exist in order
            var wellExistOrder = _context.Order.Count(o => o.WellId == id && !o.IsDeleted);

            if (wellExistOrder > 0)
            {
                return BadRequest("Not able to delete Well as it has reference in order table");
            }

                _context.Well.Remove(well);
                await _context.SaveChangesAsync();

            return Ok(well);
        }

        // Check well Exist or Not
        private bool WellExists(int id)
        {
            return _context.Well.Any(e => e.Id == id);
        }

    }
}