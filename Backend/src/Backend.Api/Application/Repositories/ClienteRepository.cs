using Backend.Api.Application.DTOs.Request;
using Backend.Api.Application.DTOs.Response;
using Backend.Api.Application.Interfaces;
using Backend.Api.Domain.Entities;
using Backend.Api.Infrastructure;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using Microsoft.EntityFrameworkCore;

namespace Backend.Api.Application.Repositories
{
    public class ClienteRepository : IClienteRepository
    {
        private readonly ApplicationDbContext _context;

        public ClienteRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Cliente> GetByIdAsync(int id)
        {
            var cliente = await _context
                .Clientes.Where(c => c.PersonaId == id)
                .FirstOrDefaultAsync();
            if (cliente == null)
            {
                throw new Exception("Cliente no encontrado.");
            }
            return cliente;
        }

        public async Task<IEnumerable<Cliente>> GetAllAsync()
        {
            var clientes = await _context.Clientes.OrderBy(c => c.Nombre).ToListAsync();

            return clientes;
        }

        public async Task<IEnumerable<EstadoCuentaDTO>> GetEstadoCuentaAsync(
            int id,
            DateTime fechaInicio,
            DateTime fechaFin
        )
        {
            var cliente = await _context.Clientes.FirstOrDefaultAsync(c => c.PersonaId == id);
            if (cliente == null)
            {
                throw new Exception("El cliente seleccionado no existe.");
            }

            var reporte = await _context
                .Cuentas.Where(c => c.ClienteId == id)
                .Select(c => new EstadoCuentaDTO
                {
                    NumeroCuenta = c.NumeroCuenta,

                    TotalDebito =
                        c.Movimientos.Where(m =>
                                m.Fecha >= fechaInicio
                                && m.Fecha <= fechaFin
                                && m.TipoMovimiento == "D"
                            )
                            .Sum(m => (decimal?)m.Valor)
                        ?? 0,

                    TotalCredito =
                        c.Movimientos.Where(m =>
                                m.Fecha >= fechaInicio
                                && m.Fecha <= fechaFin
                                && m.TipoMovimiento == "C"
                            )
                            .Sum(m => (decimal?)m.Valor)
                        ?? 0,

                    Saldo =
                        c.Movimientos.Where(m => m.Fecha <= fechaFin)
                            .OrderByDescending(m => m.Fecha)
                            .Select(m => (decimal?)m.Saldo)
                            .FirstOrDefault()
                        ?? 0,
                })
                .ToListAsync();

            return reporte;
        }

        public async Task<string> GetEstadoCuentaPdfAsync(
            int id,
            DateTime fechaInicio,
            DateTime fechaFin
        )
        {
            var estadoCuenta = await GetEstadoCuentaAsync(id, fechaInicio, fechaFin);

            using var ms = new MemoryStream();
            using (var writer = new PdfWriter(ms))
            {
                using var pdf = new PdfDocument(writer);
                var doc = new Document(pdf);

                doc.Add(new Paragraph($"Reporte Estado de Cuenta"));
                doc.Add(new Paragraph($"Rango: {fechaInicio:yyyy-MM-dd} - {fechaFin:yyyy-MM-dd}"));
                doc.Add(new Paragraph("\nCuentas:"));

                foreach (var detalle in estadoCuenta)
                {
                    doc.Add(
                        new Paragraph(
                            $"Cuenta: {detalle.NumeroCuenta}, Saldo: {detalle.Saldo}, Débitos: {detalle.TotalDebito}, Créditos: {detalle.TotalCredito}"
                        )
                    );
                }

                doc.Add(new Paragraph($"Total Débitos: {estadoCuenta.Sum(c => c.TotalDebito)}"));
                doc.Add(new Paragraph($"Total Créditos: {estadoCuenta.Sum(c => c.TotalCredito)}"));

                doc.Close();
            }

            var pdfBase64 = Convert.ToBase64String(ms.ToArray());

            return pdfBase64;
        }

        public async Task AddAsync(AddClienteDTO cliente)
        {
            var nuevoCliente = new Cliente
            {
                Nombre = cliente.Nombre,
                Genero = cliente.Genero,
                Edad = cliente.Edad,
                Identificacion = cliente.Identificacion,
                Direccion = cliente.Direccion,
                Telefono = cliente.Telefono,
                Contrasena = cliente.Contrasena,
                Estado = true,
            };

            _context.Clientes.Add(nuevoCliente);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(int id, UpdateClienteDTO cliente)
        {
            var clienteEnDb = await _context.Clientes.FirstOrDefaultAsync(c => c.PersonaId == id);
            if (clienteEnDb == null)
            {
                throw new Exception("El cliente seleccionado no existe.");
            }

            clienteEnDb.Nombre = cliente.Nombre;
            clienteEnDb.Genero = cliente.Genero;
            clienteEnDb.Edad = cliente.Edad;
            clienteEnDb.Identificacion = cliente.Identificacion;
            clienteEnDb.Direccion = cliente.Direccion;
            clienteEnDb.Telefono = cliente.Telefono;
            clienteEnDb.Contrasena = cliente.Contrasena;
            clienteEnDb.Estado = cliente.Estado;

            _context.Update(clienteEnDb);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var cliente = await _context.Clientes.FirstOrDefaultAsync(c => c.PersonaId == id);

            if (cliente == null)
            {
                throw new Exception("El cliente seleccionado no existe.");
            }

            _context.Remove(cliente);
            await _context.SaveChangesAsync();
        }
    }
}
