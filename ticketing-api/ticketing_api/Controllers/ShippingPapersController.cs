﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sieve.Models;
using Sieve.Services;
using System.Linq;
using System.Threading.Tasks;
using ticketing_api.Data;
using ticketing_api.Infrastructure;
using ticketing_api.Models;
using ticketing_api.Services;

namespace ticketing_api.Controllers
{
    /// <summary>
    /// Manage shipping paper content
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ShippingPapersController : BaseController.BaseController
    {
        private readonly ILogger<ShippingPapersController> _logger;
        private readonly ShippingPaperService _shippingPaperService;

        public ShippingPapersController(ApplicationDbContext context, ILogger<ShippingPapersController> logger, IEmailSender emailSender, ISieveProcessor sieveProcessor)
            : base(context, emailSender, sieveProcessor)
        {
            _logger = logger;
            _shippingPaperService = new ShippingPaperService(_context, _sieveProcessor);
        }

        [HttpGet]
        public async Task<IActionResult> GetShippingPaper([FromQuery] SieveModel sieveModel)
        {
            var isPermisssion = await CheckPermissions("SHIPPINGPAPERS", "Read");

            if (!isPermisssion)
            {
                return BadRequest("Read permission not allowed");
            }

            var data = await _shippingPaperService.GetListViewAsync(sieveModel);
            return Ok(data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetShippingPaper([FromRoute] int id)
        {
            var isPermisssion = await CheckPermissions("SHIPPINGPAPERS", "Read");

            if (!isPermisssion)
            {
                return BadRequest("Read permission not allowed");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var shippingPaper = await _context.ShippingPaper.FindAsync(id);

            if (shippingPaper == null)
            {
                return NotFound("ShippingPaper Id not found");
            }

            return Ok(shippingPaper);
        }

        [HttpPost]
        public async Task<IActionResult> PostShippingPaper([FromBody] ShippingPaper shippingPaper)
        {
            var isPermisssion = await CheckPermissions("SHIPPINGPAPERS", "Create");

            if (!isPermisssion)
            {
                return BadRequest("Create permission not allowed");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //check if shipping paper for market already exists

            _context.ShippingPaper.Add(shippingPaper);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetShippingPaper", new { id = shippingPaper.Id }, shippingPaper);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutShippingPaper([FromRoute] int id, [FromBody] ShippingPaper shippingPaper)
        {
            var isPermisssion = await CheckPermissions("SHIPPINGPAPERS", "Update");

            if (!isPermisssion)
            {
                return BadRequest("Update permission not allowed");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != shippingPaper.Id)
            {
                return BadRequest("Requested shipping paper id does not match with querystring id");
            }

            //check if shipping paper for market already exists

            _context.Entry(shippingPaper).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(shippingPaper);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteShippingPaper([FromRoute] int id)
        {
            var isPermisssion = await CheckPermissions("SHIPPINGPAPERS", "Delete");

            if (!isPermisssion)
            {
                return BadRequest("Delete permission not allowed");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var shippingPaper = await _context.ShippingPaper.FindAsync(id);
            if (shippingPaper == null)
            {
                return NotFound("ShippingPaper Id not found");
            }

            _context.ShippingPaper.Remove(shippingPaper);
            await _context.SaveChangesAsync();

            return Ok(shippingPaper);
        }
    }
}