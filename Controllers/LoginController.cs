using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging; // Import logging namespace
using System.Linq;
using Shareplus.DataLayer.Data;
using Shareplus.Models;
using Shareplus.Model;

namespace Shareplus.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly ILogger<LoginController> _logger; // Declare logger

        public LoginController(DataContext context, ILogger<LoginController> logger) // Inject logger
        {
            _context = context;
            _logger = logger;
        }

        // Login endpoint
                // Login endpoint
        [HttpPost("login")]
        public IActionResult Login([FromBody] User login)
        {
            _logger.LogInformation("Login attempt for user: {Username}", login.Username);

            // Check if the user is an admin
            var admin = _context.Admins.FirstOrDefault(u => u.Username == login.Username && u.Password == login.Password);
            if (admin != null)
            {
                _logger.LogInformation("Admin login successful for user: {Username}", login.Username);
                return Ok(new { Role = "Admin", Message = "Login Successful, Now You can see AdminHomescreen" });
            }

            // Check if the user is an associate
            var associate = _context.Associates.FirstOrDefault(u => u.Username == login.Username && u.Password == login.Password);
            if (associate != null)
            {
                _logger.LogInformation("Associate login successful for user: {Username}", login.Username);
                return Ok(new { Role = "Associate", Message = "Login Successful, Now you can see AssociateHomeScreen" });
            }

            _logger.LogWarning("Login failed for user: {Username}", login.Username);
            return Unauthorized();
        }

        // // Logout endpoint
        // [HttpPost("logout")]
        // public IActionResult Logout()
        // {
        //     _logger.LogInformation("Logout attempt");
        //     // Perform logout actions here
        //     return Ok("Logout Successful");
        // }
        // Logout endpoint

        // Logout endpoint
        [HttpPost("logout")]
        public IActionResult Logout()
        {
            
            string username = User.Identity.Name; // Get the username of the logged-in user from the JWT token
            
            _logger.LogInformation($"Logout attempt - User '{username}' logged out successfully");
            
            return Ok(new { message = "Logout successful" }); // Redirect to the login page after logout
        }

        // Register endpoint for admin
        [HttpPost("register-admin")]
        public IActionResult RegisterAdmin([FromBody] Admin admin)
        {
            _logger.LogInformation("Register admin attempt for user: {Username}", admin.Username);

            // Check if the admin already exists
            var existingAdmin = _context.Admins.FirstOrDefault(a => a.Username == admin.Username);
            if (existingAdmin != null)
            {
                _logger.LogWarning("Admin registration failed: user already exists: {Username}", admin.Username);
                return Conflict("Admin already exists");
            }

            // Add the admin to the database
            _context.Admins.Add(admin);
            _context.SaveChanges();

            _logger.LogInformation("Admin registered successfully: {Username}", admin.Username);
            return Ok("Admin registered successfully");
        }

        // Register endpoint for associate
        [HttpPost("add-associate")]
        public IActionResult AddAssociate([FromBody] Associate associate)
        {
            _logger.LogInformation("Add associate attempt for user: {Username}", associate.Username);

            // Add the associate to the database
            _context.Associates.Add(associate);
            _context.SaveChanges();

            _logger.LogInformation("Associate added successfully: {Username}", associate.Username);
            return Ok("Associate added successfully");
        }
    }
}