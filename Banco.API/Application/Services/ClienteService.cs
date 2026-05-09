using Banco.API.Application.DTOs;
using Banco.API.Domain.Entities;
using Banco.API.Infrastructure.Repositories;

namespace Banco.API.Application.Services
{
    public class ClienteService
    {
        private readonly ClienteRepository _clienteRepository;
        private readonly AgenciaRepository _agenciaRepository;

        public ClienteService(
            ClienteRepository clienteRepository,
            AgenciaRepository agenciaRepository)
        {
            _clienteRepository = clienteRepository;
            _agenciaRepository = agenciaRepository;
        }

        public async Task<List<ClienteResponseDTO>> BuscarTodos()
        {
            var clientes = await _clienteRepository.GetAllAsync();

            return clientes
                .Select(MapToResponse)
                .ToList();
        }

        public async Task<ClienteResponseDTO> CriarPessoaFisica(PessoaFisica cliente)
        {
            var agenciaExiste =
                await _agenciaRepository.ExistsAsync(cliente.AgenciaId);

            if (!agenciaExiste)
                throw new Exception("Agência não encontrada");

            var cpfExiste =
                await _clienteRepository.ExistsByCpfAsync(cliente.CPF);

            if (cpfExiste)
                throw new Exception("CPF já cadastrado");

            await _clienteRepository.AddAsync(cliente);

            return MapToResponse(cliente);
        }

        public async Task<ClienteResponseDTO> CriarPessoaJuridica(PessoaJuridica cliente)
        {
            var agenciaExiste =
                await _agenciaRepository.ExistsAsync(cliente.AgenciaId);

            if (!agenciaExiste)
                throw new Exception("Agência não encontrada");

            var cnpjExiste =
                await _clienteRepository.ExistsByCnpjAsync(cliente.CNPJ);

            if (cnpjExiste)
                throw new Exception("CNPJ já cadastrado");

            await _clienteRepository.AddAsync(cliente);

            return MapToResponse(cliente);
        }

        public async Task<ClienteResponseDTO> BuscarPorId(int id)
        {
            var cliente =
                await _clienteRepository.GetByIdAsync(id);

            if (cliente == null)
                throw new Exception("Cliente não encontrado");

            return MapToResponse(cliente);
        }

        public async Task<ClienteResponseDTO> AtualizarPF(
            int id,
            ClientePFUpdateDTO dto)
        {
            var cliente =
                await _clienteRepository.GetByIdAsync(id);

            if (cliente == null || cliente is not PessoaFisica pf)
                throw new Exception("Pessoa Física não encontrada");

            pf.CPF = dto.CPF;
            pf.DataNascimento = dto.DataNascimento;
            pf.AgenciaId = dto.AgenciaId;

            await _clienteRepository.UpdateAsync(pf);

            return MapToResponse(pf);
        }

        public async Task<ClienteResponseDTO> AtualizarPJ(
            int id,
            ClientePJUpdateDTO dto)
        {
            var cliente =
                await _clienteRepository.GetByIdAsync(id);

            if (cliente == null || cliente is not PessoaJuridica pj)
                throw new Exception("Pessoa Jurídica não encontrada");

            pj.CNPJ = dto.CNPJ;
            pj.RazaoSocial = dto.RazaoSocial;
            pj.AgenciaId = dto.AgenciaId;

            await _clienteRepository.UpdateAsync(pj);

            return MapToResponse(pj);
        }

        public async Task Deletar(int id)
        {
            var cliente =
                await _clienteRepository.GetByIdAsync(id);

            if (cliente == null)
                throw new Exception("Cliente não encontrado");

            await _clienteRepository.DeleteAsync(cliente);
        }

        private ClienteResponseDTO MapToResponse(Cliente cliente)
        {
            if (cliente is PessoaFisica pf)
            {
                return new ClienteResponseDTO
                {
                    Id = pf.Id,
                    AgenciaId = pf.AgenciaId,
                    Tipo = "PF",
                    CPF = pf.CPF,
                    DataNascimento = pf.DataNascimento
                };
            }

            if (cliente is PessoaJuridica pj)
            {
                return new ClienteResponseDTO
                {
                    Id = pj.Id,
                    AgenciaId = pj.AgenciaId,
                    Tipo = "PJ",
                    CNPJ = pj.CNPJ,
                    RazaoSocial = pj.RazaoSocial
                };
            }

            throw new Exception("Tipo de cliente inválido");
        }
    }
}