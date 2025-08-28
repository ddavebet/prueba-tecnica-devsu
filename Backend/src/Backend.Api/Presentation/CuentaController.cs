using Backend.Api.Application.DTOs.Request;
using Backend.Api.Application.Interfaces;
using Backend.Api.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Api.Presentation
{
    [Route("api/cuentas")]
    [ApiController]
    public class CuentaController : ControllerBase
    {
        private readonly ICuentaRepository _cuentaRepository;

        public CuentaController(ICuentaRepository cuentaRepository)
        {
            _cuentaRepository = cuentaRepository;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Cuenta>> GetById(int id)
        {
            try
            {
                var cuenta = await _cuentaRepository.GetByIdAsync(id);
                return Ok(cuenta);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet]
        public async Task<ActionResult<List<Cuenta>>> GetAll()
        {
            try
            {
                var cuentas = await _cuentaRepository.GetAllAsync();
                return Ok(cuentas);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult> Add(AddCuentaDTO cuenta)
        {
            try
            {
                await _cuentaRepository.AddAsync(cuenta);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, UpdateCuentaDTO cuenta)
        {
            try
            {
                await _cuentaRepository.UpdateAsync(id, cuenta);
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
                await _cuentaRepository.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
