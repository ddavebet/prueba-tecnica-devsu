using Backend.Api.Application.DTOs.Request;
using Backend.Api.Application.Interfaces;
using Backend.Api.Domain.Entities;
using Backend.Api.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Backend.Api.Application.Repositories
{
    public class CuentaRepository : ICuentaRepository
    {
        private readonly ApplicationDbContext _context;

        public CuentaRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Cuenta> GetByIdAsync(int id)
        {
            var cuenta = await _context
                .Cuentas.Include(c => c.Cliente)
                .Where(c => c.CuentaId == id)
                .FirstOrDefaultAsync();
            if (cuenta == null)
            {
                throw new Exception("Cuenta no encontrada.");
            }
            return cuenta;
        }

        public async Task<IEnumerable<Cuenta>> GetAllAsync()
        {
            var cuentas = await _context
                .Cuentas.Include(c => c.Cliente)
                .OrderBy(c => c.NumeroCuenta)
                .ToListAsync();

            return cuentas;
        }

        public async Task AddAsync(AddCuentaDTO cuenta)
        {
            var nuevoCuenta = new Cuenta
            {
                NumeroCuenta = cuenta.NumeroCuenta,
                TipoCuenta = cuenta.TipoCuenta,
                SaldoInicial = cuenta.SaldoInicial,
                ClienteId = cuenta.ClienteId,
                Estado = true,
            };

            _context.Cuentas.Add(nuevoCuenta);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(int id, UpdateCuentaDTO cuenta)
        {
            var cuentaEnDb = await _context.Cuentas.FirstOrDefaultAsync(c => c.CuentaId == id);
            if (cuentaEnDb == null)
            {
                throw new Exception("La cuenta seleccionada no existe.");
            }

            cuentaEnDb.NumeroCuenta = cuenta.NumeroCuenta;
            cuentaEnDb.TipoCuenta = cuenta.TipoCuenta;
            cuentaEnDb.SaldoInicial = cuenta.SaldoInicial;
            cuentaEnDb.ClienteId = cuenta.ClienteId;
            cuentaEnDb.Estado = cuenta.Estado;

            _context.Update(cuentaEnDb);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var cuenta = await _context.Cuentas.FirstOrDefaultAsync(c => c.CuentaId == id);

            if (cuenta == null)
            {
                throw new Exception("La cuenta seleccionada no existe.");
            }

            _context.Remove(cuenta);
            await _context.SaveChangesAsync();
        }
    }
}
