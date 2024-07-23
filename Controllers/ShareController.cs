using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shareplus.DataLayer.Data;
using Shareplus.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shareplus.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssociateController : ControllerBase
    {
        private readonly DataContext _context;

        public AssociateController(DataContext context)
        {
            _context = context;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllAssociates() 
        {
            var associates = await _context.Associates.ToListAsync(); //get all associates
            return Ok(associates);
        }

        [HttpPost("enable-access")]
        public async Task<IActionResult> EnableAccess([FromBody] List<int> associateIds)
        {
            var associates = await _context.Associates.Where(a => associateIds.Contains(a.Id)).ToListAsync();

            foreach (var associate in associates)
            {
                associate.IsAuthorized = true;
            }

            await _context.SaveChangesAsync();
            return Ok("Access enabled for selected associates.");
        }

        [HttpPost("disable-access")]
        public async Task<IActionResult> DisableAccess([FromBody] List<int> associateIds)
        {
           
            var associates = await _context.Associates
                .Where(a => associateIds.Contains(a.Id) && a.IsAuthorized)  // only fetch those with access enabled
                .ToListAsync();

            if (associates.Count == 0)
            {
                return NotFound("No authorized associates found with the provided IDs.");
            }

            
            foreach (var associate in associates)
            {
                associate.IsAuthorized = false;
            }

        
            await _context.SaveChangesAsync();
            return Ok("Access disabled for selected associates.");
        }


        
    }
}