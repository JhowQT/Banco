using Banco.API.Application.DTOs;
using Banco.API.Application.Services;
using Banco.API.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Banco.API.Controllers
{
    [ApiController]
    [Route("api/contratacoes")]
    public class ContratacoesController : ControllerBase
    {
        private readonly ContratacaoService _contratacaoService;

        public ContratacoesController(
            ContratacaoService contratacaoService)
        {
            _contratacaoService = contratacaoService;
        }

        [HttpGet]
        public async Task<IActionResult> BuscarTodos()
        {
            var contratacoes =
                await _contratacaoService.BuscarTodos();

            return Ok(contratacoes);
        }

        [HttpPost]
        public async Task<IActionResult> Criar(
            [FromBody] ContratacaoCreateDTO dto)
        {
            var contratacao = new Contratacao
            {
                ClienteId = dto.ClienteId,
                ProdutoId = dto.ProdutoId
            };

            var result =
                await _contratacaoService.Criar(contratacao);

            return AcceptedAtAction(
                nameof(BuscarPorId),
                new { id = result.Id },
                result
            );
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> BuscarPorId(int id)
        {
            var contratacao =
                await _contratacaoService.BuscarPorId(id);

            return Ok(contratacao);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Atualizar(
            int id,
            [FromBody] ContratacaoUpdateDTO dto)
        {
            var contratacao =
                await _contratacaoService.Atualizar(id, dto);

            return Ok(contratacao);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Deletar(int id)
        {
            await _contratacaoService.Deletar(id);

            return NoContent();
        }
    }
}