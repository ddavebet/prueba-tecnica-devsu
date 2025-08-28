using Backend.Api.Application.DTOs.Request;
using Backend.Api.Application.DTOs.Response;
using Backend.Api.Application.Interfaces;
using Backend.Api.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Api.Presentation
{
    [Route("api/clientes")]
    [ApiController]
    public class ClienteController : Controller
    {
        private readonly IClienteRepository _clienteRepository;

        public ClienteController(IClienteRepository clienteRepository)
        {
            _clienteRepository = clienteRepository;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Cliente>> GetById(int id)
        {
            try
            {
                var cliente = await _clienteRepository.GetByIdAsync(id);
                return Ok(cliente);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet]
        public async Task<ActionResult<List<Cliente>>> GetAll()
        {
            try
            {
                var clientes = await _clienteRepository.GetAllAsync();
                return Ok(clientes);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("{id}/reportes")]
        public async Task<ActionResult<IEnumerable<EstadoCuentaDTO>>> GetEstadoCuenta(
            int id,
            [FromQuery] string fechas
        )
        {
            try
            {
                var partes = fechas.Split('_');
                if (partes.Length != 2)
                {
                    return BadRequest(
                        "El formato de las fechas es inválido, use: yyyy-MM-dd_yyyy-MM-dd"
                    );
                }

                var fechaInicio = DateTime.Parse(partes[0]);
                var fechaFin = DateTime.Parse(partes[1]);

                var estadoCuenta = await _clienteRepository.GetEstadoCuentaAsync(
                    id,
                    fechaInicio,
                    fechaFin
                );
                return Ok(estadoCuenta);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("{id}/reportes/pdf")]
        public async Task<ActionResult<string>> GetEstadoCuentaPdf(
            int id,
            [FromQuery] string fechas
        )
        {
            try
            {
                var partes = fechas.Split('_');
                if (partes.Length != 2)
                {
                    return BadRequest(
                        "El formato de las fechas es inválido, use: yyyy-MM-dd_yyyy-MM-dd"
                    );
                }

                var fechaInicio = DateTime.Parse(partes[0]);
                var fechaFin = DateTime.Parse(partes[1]);

                var estadoCuenta = await _clienteRepository.GetEstadoCuentaPdfAsync(
                    id,
                    fechaInicio,
                    fechaFin
                );
                return Ok(new { pdf = estadoCuenta });
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult> Add(AddClienteDTO cliente)
        {
            try
            {
                await _clienteRepository.AddAsync(cliente);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, UpdateClienteDTO cliente)
        {
            try
            {
                await _clienteRepository.UpdateAsync(id, cliente);
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
                await _clienteRepository.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
