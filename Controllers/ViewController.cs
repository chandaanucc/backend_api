using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shareplus.DataLayer.Data;
using Shareplus.DataLayer.DTOs;
using Shareplus.DataLayer.Models;
using System.IO;
using System.Threading.Tasks;

namespace Shareplus.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ViewFilesController : ControllerBase
    {
        private readonly DataContext _context;

        public ViewFilesController(DataContext context)
        {
            _context = context;
        }

        // [HttpGet("view/{id}")]
        // public async Task<IActionResult> ViewPdf(int id)
        // {
        //     try
        //     {
        //         var pdfFile = await _context.FileUploads.FindAsync(id);
        //         if (pdfFile == null)
        //             return NotFound();

        //         return File(pdfFile.Data, "application/pdf");
        //     }
        //     catch (Exception ex)
        //     {
        //         // Log the exception or handle it as needed
        //         return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred: {ex.Message}");
        //     }
        // }
        [HttpGet("view/latest")]
        public async Task<IActionResult> ViewLatestPdf( String username)
        {
            
            var associate = await _context.Associates.FirstOrDefaultAsync(a => a.Username == username);

            if (associate == null || !associate.IsAuthorized)
            {
                return Unauthorized("You do not have access to view this file.");
            }

            var pdfFile = await _context.FileUploads
                .OrderByDescending(p => p.Id)
                .FirstOrDefaultAsync();

            if (pdfFile == null)
                return NotFound();

            //ILogger.LogInformation("Successfully viewed the latest file with ID {FileId}", pdfFile.Id); // Log custom message
            return File(pdfFile.Data, "application/pdf", pdfFile.FileName);
        }
    }
    }
