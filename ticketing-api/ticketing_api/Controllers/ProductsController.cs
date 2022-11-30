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
    public class ProductsController : BaseController.BaseController
    {
        private readonly ILogger<ProductsController> _logger;
        private readonly ProductService _productService;

        public ProductsController(ApplicationDbContext context, ILogger<ProductsController> logger, IEmailSender emailSender, ISieveProcessor sieveProcessor) : base(context, emailSender, sieveProcessor)
        {
            _logger = logger;
            _productService = new ProductService(_context, sieveProcessor);
        }

        // GET: api/Products
        [HttpGet]
        public async Task<IActionResult> GetProductAsync([FromQuery]SieveModel sieveModel)
        {
            var isPermisssion = await CheckPermissions("PRODUCTS", "Read");

            if (!isPermisssion)
            {
                return BadRequest("Read permission not allowed");
            }   

            PagingResults<ProductView> product = await _productService.GetProductViewAsync(sieveModel);
            return Ok(product);
        }

        // GET: api/Products/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct([FromRoute] int id)
        {
            var isPermisssion = await CheckPermissions("PRODUCTS", "Read");

            if (!isPermisssion)
            {
                return BadRequest("Read permission not allowed");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var product = await _context.Product.FindAsync(id);

            if (product == null)
            {
                return NotFound("Product Id not found");
            }

            return Ok(product);
        }

        // POST: api/Products
        [HttpPost]
        public async Task<IActionResult> PostProductAsync([FromBody] Product product)
        {
            var isPermisssion = await CheckPermissions("PRODUCTS", "Create");

            if (!isPermisssion)
            {
                return BadRequest("Create permission not allowed");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var productNameExists = await _context.Product.FirstOrDefaultAsync(p => p.Name.Equals(product.Name,StringComparison.OrdinalIgnoreCase));
            if (productNameExists != null)
            {
                return BadRequest("Product name already exists");
            }

            _context.Product.Add(product);
            await _context.SaveChangesAsync();

            ProductView productView = _productService.PostProduct(product);

            return CreatedAtAction("GetProduct", new { id = product.Id }, productView);
        }

        // PUT: api/Products/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProductAsync([FromRoute] int id, [FromBody] Product product)
        {
            var isPermisssion = await CheckPermissions("PRODUCTS", "Update");

            if (!isPermisssion)
            {
                return BadRequest("Update permission not allowed");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != product.Id)
            {
                return BadRequest("Requested product id does not match with querystring id");
            }

            try
            {
                var productName = _context.Product.Where(c => c.Id == product.Id).Select(c => c.Name).Single();
                if (productName == product.Name)
                {
                    _context.Entry(product).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                }
                else
                {
                    var productNameExists = _context.Product.Count(p=> p.Id != product.Id && p.Name.Equals(product.Name, StringComparison.OrdinalIgnoreCase));
                    if (productNameExists > 0)
                    {
                        return BadRequest("Product name already exists");
                    }
                    else
                    {
                        _context.Entry(product).State = EntityState.Modified;
                        await _context.SaveChangesAsync();
                    }
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
                {
                    return NotFound("Product Id not found");
                }
                throw;
            }

            ProductView productView = _productService.PostProduct(product);

            return Ok(productView);
        }

        // DELETE: api/Products/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProductAsync([FromRoute] int id)
        {
            var isPermisssion = await CheckPermissions("PRODUCTS", "Delete");

            if (!isPermisssion)
            {
                return BadRequest("Delete permission not allowed");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var product = await _context.Product.FindAsync(id);
            if (product == null)
            {
                return NotFound("Product Id not found");
            }

            //check the Product Id exist in ProductTax 
            var productExistProductTax = _context.ProductTax.Count(pt => pt.ProductId == id && !pt.IsDeleted);

            if (productExistProductTax > 0)
            {
                return BadRequest("Not able to delete product as it has reference in producttax table");
            }

            //check the Product Id exist in TicketProduct 
            var productExistTicketProduct = _context.TicketProduct.Count(tp => tp.ProductId == id && !tp.IsDeleted);

            if(productExistTicketProduct > 0)
            {
                return BadRequest("Not able to delete product as it has reference in ticketproduct table");
            }

                _context.Product.Remove(product);
                await _context.SaveChangesAsync();
          
            return Ok(product);
        }

        // Check Product Exist or Not
        private bool ProductExists(int id)
        {
            return _context.Product.Any(e => e.Id == id);
        }
    }
}