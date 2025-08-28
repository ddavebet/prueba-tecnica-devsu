using Backend.Api.Application.DTOs.Request;
using Backend.Api.Application.Interfaces;
using Backend.Api.Domain.Entities;
using Backend.Api.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Backend.Api.Application.Repositories
{
    public class MovimientoRepository : IMovimientoRepository
    {
        const decimal LIMITE_SALDO = 1000;

        private readonly ApplicationDbContext _context;

        public MovimientoRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Movimiento> GetByIdAsync(int id)
        {
            var movimiento = await _context
                .Movimientos.Where(m => m.MovimientoId == id)
                .Include(m => m.Cuenta)
                .ThenInclude(c => c.Cliente)
                .FirstOrDefaultAsync();
            if (movimiento == null)
            {
                throw new Exception("Movimiento no encontrado.");
            }
            return movimiento;
        }

        public async Task<IEnumerable<Movimiento>> GetAllAsync()
        {
            var movimientos = await _context
                .Movimientos.Include(m => m.Cuenta)
                .ThenInclude(c => c.Cliente)
                .OrderBy(m => m.Fecha)
                .ToListAsync();

            return movimientos;
        }

        public async Task AddAsync(AddMovimientoDTO movimiento)
        {
            var nuevoMovimiento = new Movimiento
            {
                Fecha = DateTime.Now,
                TipoMovimiento = movimiento.TipoMovimiento,
                Valor = Math.Abs(movimiento.Valor),
                CuentaId = movimiento.CuentaId,
            };

            // Verificar límite diario para débitos
            decimal totalDiarioDebitos;
            if (nuevoMovimiento.TipoMovimiento == "D")
            {
                totalDiarioDebitos = await _context
                    .Movimientos.Where(m =>
                        m.CuentaId == nuevoMovimiento.CuentaId
                        && m.TipoMovimiento == "D"
                        && m.Fecha.Date == DateTime.Now.Date
                    )
                    .SumAsync(m => m.Valor);
                if (totalDiarioDebitos >= LIMITE_SALDO)
                {
                    throw new Exception("Cupo diario excedido.");
                }
                if (totalDiarioDebitos + nuevoMovimiento.Valor > LIMITE_SALDO)
                {
                    throw new Exception("Valor de la transacción excede cupo diario.");
                }
            }

            // Calcular nuevo saldo
            decimal nuevoSaldo = 0;
            var ultimoMovimiento = await _context
                .Movimientos.Where(m => m.CuentaId == nuevoMovimiento.CuentaId)
                .OrderByDescending(m => m.Fecha)
                .FirstOrDefaultAsync();
            if (ultimoMovimiento != null)
            {
                // Si existe un último movimiento, usar su saldo
                nuevoSaldo = ultimoMovimiento.Saldo;
            }
            else
            {
                // Si no existe un último movimiento, usar saldo inicial
                var saldoInicial = await _context
                    .Cuentas.Where(c => c.CuentaId == nuevoMovimiento.CuentaId)
                    .Select(c => c.SaldoInicial)
                    .FirstOrDefaultAsync();
                nuevoSaldo = saldoInicial;
            }

            // Validar saldo disponible para débitos cuando es cero
            if (nuevoSaldo == 0 && nuevoMovimiento.TipoMovimiento == "D")
            {
                throw new Exception("Saldo no disponible.");
            }

            // Actualizar saldo
            nuevoSaldo +=
                nuevoMovimiento.TipoMovimiento == "C"
                    ? nuevoMovimiento.Valor
                    : -nuevoMovimiento.Valor;

            // Validar saldo disponible para débitos cuando es menor que cero
            if (nuevoSaldo < 0)
            {
                throw new Exception("Valor de la transacción excede saldo disponible.");
            }

            nuevoMovimiento.Saldo = nuevoSaldo;

            _context.Movimientos.Add(nuevoMovimiento);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(int id, UpdateMovimientoDTO movimiento)
        {
            var movimientoEnDb = await _context.Movimientos.FirstOrDefaultAsync(c =>
                c.MovimientoId == id
            );
            if (movimientoEnDb == null)
            {
                throw new Exception("El movimiento seleccionado no existe.");
            }

            movimientoEnDb.TipoMovimiento = movimiento.TipoMovimiento;
            movimientoEnDb.Valor = movimiento.Valor;
            movimientoEnDb.CuentaId = movimiento.CuentaId;

            _context.Update(movimientoEnDb);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var movimiento = await _context.Movimientos.FirstOrDefaultAsync(c =>
                c.MovimientoId == id
            );

            if (movimiento == null)
            {
                throw new Exception("El movimiento seleccionado no existe.");
            }

            _context.Remove(movimiento);
            await _context.SaveChangesAsync();
        }
    }
}
