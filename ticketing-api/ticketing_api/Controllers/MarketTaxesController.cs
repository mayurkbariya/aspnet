using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
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
    public class MarketTaxesController : BaseController.BaseController
    {
        private readonly ILogger<MarketTaxesController> _logger;
        private readonly MarketTaxService _marketTaxService;

        public MarketTaxesController(ApplicationDbContext context, ILogger<MarketTaxesController> logger, IEmailSender emailSender, ISieveProcessor sieveProcessor) : base(context, emailSender, sieveProcessor)
        {
            _logger = logger;
            _marketTaxService = new MarketTaxService(_context, sieveProcessor);
        }

        // GET: api/MarketTaxes
        [HttpGet]
        public async Task<IActionResult> GetMarketTax([FromQuery]SieveModel sieveModel)
        {
            var isPermisssion = await CheckPermissions("MARKETTAXES", "Read");

            if (!isPermisssion)
            {
                return BadRequest("Read permission not allowed");
            }

            PagingResults<MarketTaxView> marketTax = await _marketTaxService.GetMarketTaxViewAsync(sieveModel);
            return Ok(marketTax);
        }

        // GET: api/MarketTaxes/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetMarketTax([FromRoute] int id)
        {
            var isPermisssion = await CheckPermissions("MARKETTAXES", "Read");

            if (!isPermisssion)
            {
                return BadRequest("Read permission not allowed");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var marketTax = await _context.MarketTax.FindAsync(id);

            if (marketTax == null)
            {
                return NotFound("Market Tax Id not found");
            }

            return Ok(marketTax);
        }

        // POST: api/MarketTaxes
        [HttpPost]
        public async Task<IActionResult> PostMarketTax([FromBody] MarketTax marketTax)
        {
            var isPermisssion = await CheckPermissions("MARKETTAXES", "Create");

            if (!isPermisssion)
            {
                return BadRequest("Create permission not allowed");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.MarketTax.Add(marketTax);
            await _context.SaveChangesAsync();

            MarketTaxView marketTaxView = _marketTaxService.PostMarketTax(marketTax);

            return CreatedAtAction("GetMarketTax", new { id = marketTax.Id }, marketTaxView);
        }

        // PUT: api/MarketTaxes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMarketTax([FromRoute] int id, [FromBody] MarketTax marketTax)
        {
            var isPermisssion = await CheckPermissions("MARKETTAXES", "Update");

            if (!isPermisssion)
            {
                return BadRequest("Update permission not allowed");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != marketTax.Id)
            {
                return BadRequest("Requested markettax id does not match with querystring id");
            }

            _context.Entry(marketTax).State = EntityState.Modified;
             MarketTaxView marketTaxView;
            try
            {
                await _context.SaveChangesAsync();
                marketTaxView = _marketTaxService.PostMarketTax(marketTax);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MarketTaxExists(id))
                {
                    return NotFound("Market Tax Id not found");
                }
                else
                {
                    throw;
                }
            }

            return Ok(marketTaxView);
        }

        // DELETE: api/MarketTaxes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMarketTax([FromRoute] int id)
        {
            var isPermisssion = await CheckPermissions("MARKETTAXES", "Delete");

            if (!isPermisssion)
            {
                return BadRequest("Delete permission not allowed");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var marketTax = await _context.MarketTax.FindAsync(id);
            if (marketTax == null)
            {
                return NotFound("Market Tax Id not found");
            }

            _context.MarketTax.Remove(marketTax);
            await _context.SaveChangesAsync();

            return Ok(marketTax);
        }


        private bool MarketTaxExists(int id)
        {
            return _context.MarketTax.Any(e => e.Id == id);
        }

        /// <summary>
        /// Get markettax based on market filter
        /// </summary>
        /// <param name="marketId">market id</param>
        /// <returns></returns>
        //GET: api/Wells/1/filterwellbyrig
        [HttpGet("{marketId}/filtermarkettaxbymarket")]
        public async Task<IActionResult> GetMarketTaxByMarketId([FromRoute] int marketId)
        {
            var isPermisssion = await CheckPermissions("MARKETTAXES", "Read");

            if (!isPermisssion)
            {
                return BadRequest("Filter market permission not allowed");
            } 

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IEnumerable<MarketTaxView> marketTax = _marketTaxService.GetMarketTaxByMarketId(marketId); 
                       
            return Ok(marketTax);
        }
    }
}