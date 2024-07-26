using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shareplus.DataLayer.Data;
using Shareplus.DataLayer.DTOs;
using Shareplus.Models;

namespace Shareplus.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        private readonly DataContext _context;

        public ClientsController(DataContext context)
        {
            _context = context;
        }

        [HttpPost("add-client")]
        public async Task<ActionResult<Client>> PostClient(ClientDTO clientDto)
        {
            if (clientDto == null)
            {
                return BadRequest("Client data is null.");
            }

            var client = new Client
            {
                Name = clientDto.ClientName,
                Mail = clientDto.Mail,
                Phone = clientDto.Phone,
                Region = clientDto.Region
            };

            _context.Clients.Add(client);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetClient), new { id = client.Id }, client);
        }
        [HttpGet("get-clients")]
        public async Task<ActionResult<IEnumerable<ClientDTO>>> GetClients()
        {
            var clients = await _context.Clients.ToListAsync();
            var clientDtos = clients.Select(client => new ClientDTO
            {
                Id = client.Id,
                ClientName = client.Name,
                Mail = client.Mail,
                Phone = client.Phone,
                Region = client.Region
            }).ToList();

            return Ok(clientDtos);
        }

        [HttpGet("region")]
        public async Task<ActionResult<IEnumerable<ClientDTO>>> GetClientsByRegion([FromQuery]string region)
        {

            var clients = await _context.Clients
                                        .Where(c => c.Region.ToLower() == region.ToLower())
                                        .Select(c => new ClientDTO
                                        {
                                            Id = c.Id,
                                            ClientName = c.Name,
                                            Mail = c.Mail,
                                            Phone = c.Phone,
                                            Region = c.Region
                                        })
                                        .ToListAsync();

            if (clients == null || clients.Count == 0)
            {
                return NotFound($"No clients found in region: {region}");
            }

            return Ok(clients);
        }
    
        [HttpGet("{id}")]
        public async Task<ActionResult<Client>> GetClient(int id)
        {
            var client = await _context.Clients.FindAsync(id);

            if (client == null)
            {
                return NotFound();
            }

            return client;
        }
    }
}
