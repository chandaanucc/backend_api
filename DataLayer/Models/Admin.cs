
namespace Shareplus.Models
{
    public class Admin
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public bool IsRegistered { get; internal set; }

        
    }
}