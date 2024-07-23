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

        [HttpGet("view/latest")]
        public async Task<IActionResult> ViewLatestPdf([FromQuery] String username)
        {
            if (string.IsNullOrEmpty(username))
                {
                    return BadRequest("Username is required.");
                }
            PDFile pdfFile = null; // Declare pdfFile outside if-else blocks

            // Check if the user is an admin
            var admin = await _context.Admins.FirstOrDefaultAsync(a => a.Username == username);
            if (admin != null && admin.IsAuthorized)
            {
                pdfFile = await _context.FileUploads.OrderByDescending(p => p.Id).FirstOrDefaultAsync();
            }
            else
            {
                // Check if the user is an authorized associate
                var associate = await _context.Associates.FirstOrDefaultAsync(a => a.Username == username);
                if (associate != null && associate.IsAuthorized)
                {
                    pdfFile = await _context.FileUploads.OrderByDescending(p => p.Id).FirstOrDefaultAsync();
                }
            }

            if (pdfFile == null)
                return NotFound();

            return File(pdfFile.Data, "application/pdf", pdfFile.FileName);
        }
    }
    }
