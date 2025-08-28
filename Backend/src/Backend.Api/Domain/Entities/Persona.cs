using System.ComponentModel.DataAnnotations;

namespace Backend.Api.Domain.Entities
{
    public class Persona
    {
        [Key]
        public int PersonaId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Nombre { get; set; }

        [Required]
        [MaxLength(10)]
        public string Genero { get; set; }

        [Required]
        [Range(0, 120)]
        public int Edad { get; set; }

        [Required]
        [MaxLength(10)]
        public string Identificacion { get; set; }

        [Required]
        [MaxLength(200)]
        public string Direccion { get; set; }

        [Required]
        [MaxLength(20)]
        public string Telefono { get; set; }
    }
}
