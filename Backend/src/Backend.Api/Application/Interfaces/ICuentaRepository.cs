using Backend.Api.Application.DTOs.Request;
using Backend.Api.Domain.Entities;

namespace Backend.Api.Application.Interfaces
{
    public interface ICuentaRepository
    {
        Task<Cuenta> GetByIdAsync(int id);
        Task<IEnumerable<Cuenta>> GetAllAsync();
        Task AddAsync(AddCuentaDTO cuenta);
        Task UpdateAsync(int id, UpdateCuentaDTO cuenta);
        Task DeleteAsync(int id);
    }
}
