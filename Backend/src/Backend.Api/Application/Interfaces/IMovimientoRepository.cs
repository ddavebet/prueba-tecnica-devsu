using Backend.Api.Application.DTOs.Request;
using Backend.Api.Domain.Entities;

namespace Backend.Api.Application.Interfaces
{
    public interface IMovimientoRepository
    {
        Task<Movimiento> GetByIdAsync(int id);
        Task<IEnumerable<Movimiento>> GetAllAsync();
        Task AddAsync(AddMovimientoDTO movimiento);
        Task UpdateAsync(int id, UpdateMovimientoDTO movimiento);
        Task DeleteAsync(int id);
    }
}
