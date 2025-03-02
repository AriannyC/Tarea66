using System.ComponentModel.DataAnnotations;

namespace Tarea6.Models
{
    public class RegiUs
    {
        public int IdR { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string refreshtoken1 { get; set; }
        public DateTime TokenExpired { get; set; }

        public DateTime TokenCreated { get; set; } 
    
}
}
