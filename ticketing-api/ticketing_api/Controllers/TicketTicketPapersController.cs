using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Filters;
using ticketing_api.Infrastructure;
using ticketing_api.Services;

namespace ticketing_api.Controllers
{
    public partial class TicketsController : BaseController.BaseController
    {
        /// <summary>
        /// Generate pdf and return the pdf data as the response
        /// </summary>
        /// <param name="id"></param>
        /// <param name="startingGallons"></param>
        /// <param name="paperSize"></param>
        /// <returns></returns>
        [HttpGet("{id}/ticketpaper")]
        public async Task<IActionResult> GetTicketPaper([FromRoute] int id, [FromQuery]decimal startingGallons, [FromQuery]string paperSize)
        {
            var isPermisssion = await CheckPermissions("TICKETS", "Update");

            if (!isPermisssion)
            {
                return BadRequest("Generate ticketpaper pdf permission not allowed");
            }

            var ticketView = _orderService.GetOrderView(id);

            if (ticketView == null)
                return BadRequest("Ticket does not exists");

            if (ticketView.MarketId == null)
                return BadRequest("Ticket has no assigned market");

            var ticketPaper = _context.TicketPaper.FirstOrDefault(x => x.MarketId == ticketView.MarketId.Id);

            if (ticketPaper == null)
                return BadRequest("Ticket paper does not exists");

            var ticketTaxService = new TicketTaxService(_context, _ticketService);
            var taxes = ticketTaxService.UpdateTaxForTicket(id);
            var ticketPaperService = new TicketPaperService(_context, _sieveProcessor);

            var filepath = ticketPaperService.GeneratePdf(ticketView, startingGallons, ticketPaper, out string filename, paperSize);

            var file = System.IO.File.ReadAllBytes(filepath);
            return File(file, "application/pdf", filename);
        }


        /// <summary>
        /// Generate pdf and return link to access the pdf
        /// </summary>
        /// <param name="id"></param>
        /// <param name="startingGallons"></param>
        /// <param name="paperSize"></param>
        /// <returns></returns>
        [HttpGet("{id}/ticketpaperpath")]
        public async Task<IActionResult> GetTicketPaperUrl([FromRoute] int id, [FromQuery]decimal startingGallons, [FromQuery]string paperSize)
        {
            var isPermisssion = await CheckPermissions("TICKETS", "Update");

            if (!isPermisssion)
            {
                return BadRequest("Generate ticketpaper pdf and return path permission not allowed");
            }

            var ticketView = _orderService.GetOrderView(id);

            if (ticketView == null)
                return BadRequest("Ticket does not exist.");

            if (ticketView.MarketId == null)
                return BadRequest("Ticket does not have an assigned market.");

            var ticketPaper = _context.TicketPaper.FirstOrDefault(x => x.MarketId == ticketView.MarketId.Id);

            if (ticketPaper == null)
                return BadRequest("Ticket paper does not exist.");

            var ticketTaxService = new TicketTaxService(_context, _ticketService);
            var taxes = ticketTaxService.UpdateTaxForTicket(id);
            var ticketPaperService = new TicketPaperService(_context, _sieveProcessor);

            var filepath = ticketPaperService.GeneratePdf(ticketView, startingGallons, ticketPaper, out string filename, paperSize);
            return Ok(new { id, path = filepath });
        }

        /// <summary>
        /// Get image of printed ticket paper
        /// </summary>
        /// <param name="id">ticket id</param>
        /// <returns></returns>
        [HttpGet("{id}/ticketpaperimage")]
        public async Task<IActionResult> GetTicketPaperImage([FromRoute] int id)
        {
            var isPermisssion = await CheckPermissions("TICKETS", "Update");

            if (!isPermisssion)
            {
                return BadRequest("Read ticketpaperimage permission not allowed");
            }

            var ticketPaperService = new TicketPaperService(_context, _sieveProcessor);
            var listImages = ticketPaperService.GetTicketPaperImages(id);
            if (listImages == null || listImages.Count == 0)
                return Content("No images available.");

            var url = $"{Request.Scheme}://{Request.Host}";

            listImages.ForEach(a => a.FilePath = $"{url}/{a.FilePath.Replace("\\", "/")}");

            return Ok(listImages);
        }

        /// <summary>
        /// post image of printed ticket paper
        /// </summary>
        /// <param name="id">ticket id</param>
        /// <param name="file">image file</param>
        /// <returns></returns>
        [HttpPost("{id}/ticketpaperimage")]
        [AddSwaggerFileUploadButton]
        public async Task<IActionResult> PostTicketPaperImageAsync([FromRoute] int id, IFormFile file)
        {
            var isPermisssion = await CheckPermissions("TICKETS", "Update");

            if (!isPermisssion)
            {
                return BadRequest("Upload ticketpaperimage permission not allowed");
            }

            if (file == null) return BadRequest("File not supplied");

            var ticket = await _context.Order.FindAsync(id);
            var errorMsg = await _ticketService.CheckTicketAccessAsync(ticket, AppUser);
            if (errorMsg != null) return BadRequest(errorMsg);

            var paperService = new TicketPaperService(_context, _sieveProcessor);
            var message = await paperService.SaveImage(ticket, file);

            if (!string.IsNullOrEmpty(message)) return BadRequest(message);

            return Content("Image Uploaded Successfully");
        }

        ///// <summary>
        ///// post multiple image of printed ticket paper
        ///// </summary>
        ///// <param name="id">ticket id</param>
        ///// <param name="formCollection"></param>
        ///// <returns></returns>
        //[HttpPost("{id}/ticketpaperimages")]
        //public async Task<IActionResult> PostTicketPaperImagesAsync([FromRoute] int id, IFormCollection formCollection)
        //{
        //    const string keyName = "files";
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    List<IFormFile> files = formCollection?.Files.Where(f => f.Name == keyName).ToList();
        //    if (files == null || files.Count < 0) return BadRequest("File not supplied");
        //    var paperService = new TicketPaperService(_context, _sieveProcessor);
        //    var message = await paperService.SaveImages(id, files);

        //    if (!string.IsNullOrEmpty(message)) return BadRequest(message);

        //    return Content("Image Uploaded Successfully");
        //}
    }
}