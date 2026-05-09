using Banco.API.Application.DTOs;
using Banco.API.Domain.Entities;
using Banco.API.Infrastructure.Repositories;

namespace Banco.API.Application.Services
{
    public class AgenciaService
    {
        private readonly AgenciaRepository _agenciaRepository;

        public AgenciaService(AgenciaRepository agenciaRepository)
        {
            _agenciaRepository = agenciaRepository;
        }

        public async Task<List<AgenciaResponseDTO>> BuscarTodos()
        {
            var agencias = await _agenciaRepository.GetAllAsync();

            return agencias
                .Select(MapToResponse)
                .ToList();
        }

        public async Task<AgenciaResponseDTO> Criar(Agencia agencia)
        {
            await _agenciaRepository.AddAsync(agencia);

            return MapToResponse(agencia);
        }

        public async Task<AgenciaResponseDTO> BuscarPorId(int id)
        {
            var agencia = await _agenciaRepository.GetByIdAsync(id);

            if (agencia == null)
                throw new Exception("Agência não encontrada");

            return MapToResponse(agencia);
        }

        public async Task<AgenciaResponseDTO> Atualizar(
            int id,
            AgenciaUpdateDTO dto)
        {
            var agencia = await _agenciaRepository.GetByIdAsync(id);

            if (agencia == null)
                throw new Exception("Agência não encontrada");

            agencia.Nome = dto.Nome;

            await _agenciaRepository.UpdateAsync(agencia);

            return MapToResponse(agencia);
        }

        public async Task Deletar(int id)
        {
            var agencia = await _agenciaRepository.GetByIdAsync(id);

            if (agencia == null)
                throw new Exception("Agência não encontrada");

            await _agenciaRepository.DeleteAsync(agencia);
        }

        private AgenciaResponseDTO MapToResponse(Agencia agencia)
        {
            return new AgenciaResponseDTO
            {
                Id = agencia.Id,
                Nome = agencia.Nome
            };
        }
    }
}