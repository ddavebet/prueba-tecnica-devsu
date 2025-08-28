using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Api.Domain.Entities
{
    public class Cuenta
    {
        [Key]
        public int CuentaId { get; set; }

        [Required]
        [MaxLength(20)]
        public string NumeroCuenta { get; set; }

        [Required]
        [MaxLength(20)]
        public string TipoCuenta { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal SaldoInicial { get; set; }

        [Required]
        public bool Estado { get; set; }

        [Required]
        public int ClienteId { get; set; }

        public Cliente Cliente { get; set; }
        public List<Movimiento> Movimientos { get; set; }
    }
}
