
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shareplus.Models {

    public class Client {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; internal set; }
        public string ? Name { get; set; }
        public string ? Mail { get; set; }
        public int ? Phone { get; set; }
        public string ? Region { get; set; }

    }
}