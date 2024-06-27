using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shareplus.DataLayer.Data;
using Shareplus.DataLayer.DTOs;
using Shareplus.DataLayer.Models;
using System.IO;
using System.Threading.Tasks;

namespace Shareplus.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadFilesController : ControllerBase
    {
        private readonly DataContext _context;
        private const long MaxFileSize = 50 * 1024 * 1024; // 50 MB

        public UploadFilesController(DataContext context)
        {
            _context = context;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadPdf([FromForm] UploadDTO pdfUploadDto)
        {
            
                if (pdfUploadDto.File == null || pdfUploadDto.File.Length == 0)
                    return BadRequest("Upload a valid PDF file.");

                if (pdfUploadDto.File.Length > MaxFileSize)
                    return BadRequest($"File size must be less than {MaxFileSize / (1024 * 1024)} MB.");

                using var memoryStream = new MemoryStream();
                await pdfUploadDto.File.CopyToAsync(memoryStream);

                var pdfFile = new PDFile
                {
                    FileName = pdfUploadDto.File.FileName,
                    Data = memoryStream.ToArray()
                };

                _context.FileUploads.Add(pdfFile);
                await _context.SaveChangesAsync();

                var pdfFileDto = new UploadDTO
                {
                    //Id = pdfFile.Id,
                    FileName = pdfFile.FileName
                };

                return Ok(pdfFileDto);
            
        }

        
    }
}
