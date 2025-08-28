using System.ComponentModel.DataAnnotations;

namespace Backend.Api.Application.DTOs.Request;

public class UpdateMovimientoDTO
{
    [Required]
    [MaxLength(50)]
    public string TipoMovimiento { get; set; }

    [Required]
    public decimal Valor { get; set; }

    [Required]
    public int CuentaId { get; set; }
}
