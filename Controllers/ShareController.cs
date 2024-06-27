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
            var associates = await _context.Associates.ToListAsync();
            return Ok(associates);
        }

        
    }
}