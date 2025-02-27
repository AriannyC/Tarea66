using System.ComponentModel.DataAnnotations;

namespace Tarea6.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Debe colocar el nombre ")]

        public string Name { get; set; }
        [Required(ErrorMessage = "Debe colocar el correo ")]
        [EmailAddress(ErrorMessage = "El correo electrónico no es válido")]

        public string Email { get; set; }
        [Required(ErrorMessage = "Debe colocar fecha de na.")]

        public DateTime FechaN { get; set; }
    }
}
