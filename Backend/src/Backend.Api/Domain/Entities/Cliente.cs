using System.ComponentModel.DataAnnotations;

namespace Backend.Api.Domain.Entities
{
    public class Cliente : Persona
    {
        [Required]
        [MaxLength(100)]
        public string Contrasena { get; set; }

        [Required]
        public bool Estado { get; set; }
    }
}
