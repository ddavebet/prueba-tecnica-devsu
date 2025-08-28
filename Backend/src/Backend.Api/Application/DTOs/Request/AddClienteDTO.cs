using System.ComponentModel.DataAnnotations;

namespace Backend.Api.Application.DTOs.Request;

public class AddClienteDTO
{
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

    [Required]
    [MaxLength(100)]
    public string Contrasena { get; set; }
}
