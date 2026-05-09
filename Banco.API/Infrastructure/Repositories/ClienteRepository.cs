using Banco.API.Domain.Entities;
using Banco.API.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Banco.API.Infrastructure.Repositories
{
    public class ClienteRepository
    {
        private readonly AppDbContext _context;

        public ClienteRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Cliente>> GetAllAsync()
        {
            return await _context.Clientes
                .Include(c => c.Agencia)
                .Include(c => c.Contratacoes)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Cliente?> GetByIdAsync(int id)
        {
            return await _context.Clientes
                .Include(c => c.Agencia)
                .Include(c => c.Contratacoes)
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task AddAsync(Cliente cliente)
        {
            await _context.Clientes.AddAsync(cliente);

            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Cliente cliente)
        {
            _context.Clientes.Update(cliente);

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Cliente cliente)
        {
            _context.Clientes.Remove(cliente);

            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsByCpfAsync(string cpf)
        {
            var cliente = await _context.PessoasFisicas
                .FirstOrDefaultAsync(p => p.CPF == cpf);

            return cliente != null;
        }

        public async Task<bool> ExistsByCnpjAsync(string cnpj)
        {
            var cliente = await _context.PessoasJuridicas
                .FirstOrDefaultAsync(p => p.CNPJ == cnpj);

            return cliente != null;
        }
    }
}