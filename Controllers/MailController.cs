using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shareplus.DataLayer.Data;
using Shareplus.DataLayer.DTOs;
using Shareplus.DataLayer.Models;
using Shareplus.DataLayer.Service;
using System.IO;
using System.Threading.Tasks;
using MimeKit;
using MailKit.Net.Smtp;
using MimeKit.Utils;


namespace Shareplus.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MailController : ControllerBase
    {
        private readonly DataContext _context;

        public MailController(DataContext context)
        {
            _context = context;
        }

        
        [HttpPost("SendPdf")]
        public async Task<IActionResult> SendPdf([FromBody] SendPdfRequest request)
        {
            if (request == null || request.ClientIds == null || !request.ClientIds.Any())
                return BadRequest("Invalid request data.");

            
            var latestPdf = await _context.FileUploads
                                        .OrderByDescending(f => f.Id)
                                        .FirstOrDefaultAsync();

            if (latestPdf == null)
                return NotFound("No PDF files found.");

            var pdfId = latestPdf.Id; 

            var pdf = await _context.FileUploads.FindAsync(pdfId);

            if (pdf == null)
                return NotFound("PDF file not found.");

            var clients = await _context.Clients
                                        .Where(c => request.ClientIds.Contains(c.Id) && !string.IsNullOrEmpty(c.Mail))
                                        .ToListAsync();

            if (!clients.Any())
                return BadRequest("No valid email addresses found for the selected clients.");

            var emailAddresses = clients.Select(c => c.Mail).ToList();

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Chandaanu", "chandaanucc@gmail.com"));

            foreach (var email in emailAddresses)
            {
                message.To.Add(new MailboxAddress("", email));
            }

            message.Subject = "Share Plus Document";

            var builder = new BodyBuilder
            {
                TextBody = "Please find the attached PDF document.",
                Attachments = {
                    new MimePart("application", "pdf")
                    {
                        Content = new MimeContent(new MemoryStream(pdf.Data)),
                        ContentDisposition = new ContentDisposition(ContentDisposition.Attachment),
                        ContentTransferEncoding = ContentEncoding.Base64,
                        FileName = pdf.FileName
                    }
                }
            };

            message.Body = builder.ToMessageBody();

            using (var client = new SmtpClient())
            {
                client.ServerCertificateValidationCallback = (s, c, h, e) => true;

                await client.ConnectAsync("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
                await client.AuthenticateAsync("chandaanucc@gmail.com", "llzt yswd njkh yima");

                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }

            return Ok("Emails sent successfully.");
        }
    }

    }
    
