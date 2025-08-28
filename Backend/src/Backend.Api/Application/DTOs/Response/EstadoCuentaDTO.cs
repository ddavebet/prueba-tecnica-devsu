namespace Backend.Api.Application.DTOs.Response;

public class EstadoCuentaDTO
{
    public string NumeroCuenta { get; set; }
    public decimal Saldo { get; set; }
    public decimal TotalCredito { get; set; }
    public decimal TotalDebito { get; set; }
}
