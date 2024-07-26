
namespace Shareplus.Models
{
    
    public class Associate
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public bool IsAuthorized { get; internal set; } = false;
        public string Region { get; set; }
    }
}
