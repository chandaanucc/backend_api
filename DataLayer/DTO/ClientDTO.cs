using Microsoft.AspNetCore.Http;

namespace Shareplus.DataLayer.DTOs
{
    public class ClientDTO
    {
        public int Id { get; internal set; }
        public string ? ClientName { get; set; }
        //public string ? ContentType { get; set; }
        public string ? Mail { get; set; }
        public int ? Phone { get; set; }
        public string ? Region { get; set; }
    }
}
