using Backend.Api.Application.DTOs.Request;
using Backend.Api.Application.Interfaces;
using Backend.Api.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Api.Presentation
{
    [Route("api/movimientos")]
    [ApiController]
    public class MovimientoController : ControllerBase
    {
        private readonly IMovimientoRepository _movimientoRepository;

        public MovimientoController(IMovimientoRepository movimientoRepository)
        {
            _movimientoRepository = movimientoRepository;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Movimiento>> GetById(int id)
        {
            try
            {
                var movimiento = await _movimientoRepository.GetByIdAsync(id);
                return Ok(movimiento);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet]
        public async Task<ActionResult<List<Movimiento>>> GetAll()
        {
            try
            {
                var movimientos = await _movimientoRepository.GetAllAsync();
                return Ok(movimientos);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult> Add(AddMovimientoDTO movimiento)
        {
            try
            {
                await _movimientoRepository.AddAsync(movimiento);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, UpdateMovimientoDTO movimiento)
        {
            try
            {
                await _movimientoRepository.UpdateAsync(id, movimiento);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                await _movimientoRepository.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
