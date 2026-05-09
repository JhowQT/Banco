using Banco.API.Application.DTOs;
using Banco.API.Application.Services;
using Banco.API.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Banco.API.Controllers
{
    [ApiController]
    [Route("api/clientes")]
    public class ClientesController : ControllerBase
    {
        private readonly ClienteService _clienteService;

        public ClientesController(ClienteService clienteService)
        {
            _clienteService = clienteService;
        }

        [HttpGet]
        public async Task<IActionResult> BuscarTodos()
        {
            var clientes =
                await _clienteService.BuscarTodos();

            return Ok(clientes);
        }

        [HttpPost("pf")]
        public async Task<IActionResult> CriarPF(
            [FromBody] ClientePFCreateDTO dto)
        {
            var cliente = new PessoaFisica
            {
                CPF = dto.CPF,
                DataNascimento = dto.DataNascimento,
                AgenciaId = dto.AgenciaId
            };

            var result =
                await _clienteService.CriarPessoaFisica(cliente);

            return CreatedAtAction(
                nameof(BuscarPorId),
                new { id = result.Id },
                result
            );
        }

        [HttpPost("pj")]
        public async Task<IActionResult> CriarPJ(
            [FromBody] ClientePJCreateDTO dto)
        {
            var cliente = new PessoaJuridica
            {
                CNPJ = dto.CNPJ,
                RazaoSocial = dto.RazaoSocial,
                AgenciaId = dto.AgenciaId
            };

            var result =
                await _clienteService.CriarPessoaJuridica(cliente);

            return CreatedAtAction(
                nameof(BuscarPorId),
                new { id = result.Id },
                result
            );
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> BuscarPorId(int id)
        {
            var cliente =
                await _clienteService.BuscarPorId(id);

            return Ok(cliente);
        }

        [HttpPut("pf/{id}")]
        public async Task<IActionResult> AtualizarPF(
            int id,
            [FromBody] ClientePFUpdateDTO dto)
        {
            var cliente =
                await _clienteService.AtualizarPF(id, dto);

            return Ok(cliente);
        }

        [HttpPut("pj/{id}")]
        public async Task<IActionResult> AtualizarPJ(
            int id,
            [FromBody] ClientePJUpdateDTO dto)
        {
            var cliente =
                await _clienteService.AtualizarPJ(id, dto);

            return Ok(cliente);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Deletar(int id)
        {
            await _clienteService.Deletar(id);

            return NoContent();
        }
    }
}