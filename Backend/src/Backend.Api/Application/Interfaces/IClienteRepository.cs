using Backend.Api.Application.DTOs.Request;
using Backend.Api.Application.DTOs.Response;
using Backend.Api.Domain.Entities;

namespace Backend.Api.Application.Interfaces
{
    public interface IClienteRepository
    {
        Task<Cliente> GetByIdAsync(int id);
        Task<IEnumerable<Cliente>> GetAllAsync();
        Task<IEnumerable<EstadoCuentaDTO>> GetEstadoCuentaAsync(
            int id,
            DateTime fechaInicio,
            DateTime fechaFin
        );
        Task<string> GetEstadoCuentaPdfAsync(int id, DateTime fechaInicio, DateTime fechaFin);
        Task AddAsync(AddClienteDTO cliente);
        Task UpdateAsync(int id, UpdateClienteDTO cliente);
        Task DeleteAsync(int id);
    }
}
