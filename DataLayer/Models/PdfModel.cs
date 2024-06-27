using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shareplus.DataLayer.Models

{
    public class PDFile
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; internal set; }
        public string ? FileName { get; set; }
        public byte[] ? Data { get; set; }
        //public string ? ContentType { get; set; }
    }
}
