using System.ComponentModel.DataAnnotations;

namespace Backend.Api.Application.DTOs.Request;

public class UpdateCuentaDTO
{
    [Required]
    [MaxLength(20)]
    public string NumeroCuenta { get; set; }

    [Required]
    [MaxLength(20)]
    public string TipoCuenta { get; set; }

    [Required]
    public decimal SaldoInicial { get; set; }

    [Required]
    public int ClienteId { get; set; }

    [Required]
    public bool Estado { get; set; }
}
