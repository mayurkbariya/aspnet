﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ticketing_api.Data;
using ticketing_api.Models;

namespace ticketing_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VendorsController : ControllerBase
    {
        private readonly ILogger<VendorsController> _logger;
        private readonly ApplicationDbContext _context;

        public VendorsController(ApplicationDbContext context, ILogger<VendorsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/Vendors
        [HttpGet]
        public IEnumerable<Vendor> GetVendor()
        {
            return _context.Vendor;
        }

        // GET: api/Vendors/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetVendor([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var vendor = await _context.Vendor.FindAsync(id);

            if (vendor == null)
            {
                return NotFound();
            }

            return Ok(vendor);
        }

        // PUT: api/Vendors/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutVendor([FromRoute] int id, [FromBody] Vendor vendor)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != vendor.Id)
            {
                return BadRequest("Requested vender id does not match with querystring id");
            }

            _context.Entry(vendor).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VendorExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Vendors
        [HttpPost]
        public async Task<IActionResult> PostVendor([FromBody] Vendor vendor)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Vendor.Add(vendor);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetVendor", new { id = vendor.Id }, vendor);
        }

        // DELETE: api/Vendors/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVendor([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var vendor = await _context.Vendor.FindAsync(id);
            if (vendor == null)
            {
                return NotFound();
            }

            _context.Vendor.Remove(vendor);
            await _context.SaveChangesAsync();

            return Ok(vendor);
        }

        private bool VendorExists(int id)
        {
            return _context.Vendor.Any(e => e.Id == id);
        }
    }
}