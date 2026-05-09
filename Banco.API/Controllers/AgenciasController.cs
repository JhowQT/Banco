using Banco.API.Application.DTOs;
using Banco.API.Application.Services;
using Banco.API.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Banco.API.Controllers
{
    [ApiController]
    [Route("api/agencias")]
    public class AgenciasController : ControllerBase
    {
        private readonly AgenciaService _agenciaService;

        public AgenciasController(AgenciaService agenciaService)
        {
            _agenciaService = agenciaService;
        }

        [HttpGet]
        public async Task<IActionResult> BuscarTodos()
        {
            var agencias =
                await _agenciaService.BuscarTodos();

            return Ok(agencias);
        }

        [HttpPost]
        public async Task<IActionResult> Criar(
            [FromBody] AgenciaCreateDTO dto)
        {
            var agencia = new Agencia
            {
                Nome = dto.Nome
            };

            var result =
                await _agenciaService.Criar(agencia);

            return CreatedAtAction(
                nameof(BuscarPorId),
                new { id = result.Id },
                result
            );
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> BuscarPorId(int id)
        {
            var agencia =
                await _agenciaService.BuscarPorId(id);

            return Ok(agencia);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Atualizar(
            int id,
            [FromBody] AgenciaUpdateDTO dto)
        {
            var agencia =
                await _agenciaService.Atualizar(id, dto);

            return Ok(agencia);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Deletar(int id)
        {
            await _agenciaService.Deletar(id);

            return NoContent();
        }
    }
}