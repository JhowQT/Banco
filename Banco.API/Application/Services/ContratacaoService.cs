using Banco.API.Application.DTOs;
using Banco.API.Domain.Entities;
using Banco.API.Infrastructure.Messaging;
using Banco.API.Infrastructure.Repositories;

namespace Banco.API.Application.Services
{
    public class ContratacaoService
    {
        private readonly ContratacaoRepository _contratacaoRepository;
        private readonly ClienteRepository _clienteRepository;
        private readonly RabbitMQPublisher _publisher;

        public ContratacaoService(
            ContratacaoRepository contratacaoRepository,
            ClienteRepository clienteRepository,
            RabbitMQPublisher publisher)
        {
            _contratacaoRepository = contratacaoRepository;
            _clienteRepository = clienteRepository;
            _publisher = publisher;
        }

        public async Task<List<ContratacaoResponseDTO>> BuscarTodos()
        {
            var contratacoes =
                await _contratacaoRepository.GetAllAsync();

            return contratacoes
                .Select(MapToResponse)
                .ToList();
        }

        public async Task<ContratacaoResponseDTO> Criar(Contratacao contratacao)
        {
            var cliente =
                await _clienteRepository.GetByIdAsync(contratacao.ClienteId);

            if (cliente == null)
                throw new Exception("Cliente não encontrado");

            contratacao.Status = "PENDENTE";

            contratacao.DataCriacao = DateTime.UtcNow;

            await _contratacaoRepository.AddAsync(contratacao);

            _publisher.Publish(new
            {
                contratacao.Id,
                contratacao.ClienteId,
                contratacao.ProdutoId
            });

            return MapToResponse(contratacao);
        }

        public async Task<ContratacaoResponseDTO> BuscarPorId(int id)
        {
            var contratacao =
                await _contratacaoRepository.GetByIdAsync(id);

            if (contratacao == null)
                throw new Exception("Contratação não encontrada");

            return MapToResponse(contratacao);
        }

        public async Task<ContratacaoResponseDTO> Atualizar(
            int id,
            ContratacaoUpdateDTO dto)
        {
            var contratacao =
                await _contratacaoRepository.GetByIdAsync(id);

            if (contratacao == null)
                throw new Exception("Contratação não encontrada");

            contratacao.ClienteId = dto.ClienteId;
            contratacao.ProdutoId = dto.ProdutoId;
            contratacao.Status = dto.Status;

            await _contratacaoRepository.UpdateAsync(contratacao);

            return MapToResponse(contratacao);
        }

        public async Task Deletar(int id)
        {
            var contratacao =
                await _contratacaoRepository.GetByIdAsync(id);

            if (contratacao == null)
                throw new Exception("Contratação não encontrada");

            await _contratacaoRepository.DeleteAsync(contratacao);
        }

        private ContratacaoResponseDTO MapToResponse(
            Contratacao contratacao)
        {
            return new ContratacaoResponseDTO
            {
                Id = contratacao.Id,
                ClienteId = contratacao.ClienteId,
                ProdutoId = contratacao.ProdutoId,
                Status = contratacao.Status,
                DataCriacao = contratacao.DataCriacao
            };
        }
    }
}