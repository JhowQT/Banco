using Banco.API.Domain.Entities;
using Banco.API.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Banco.API.Infrastructure.Repositories
{
    public class ContratacaoRepository
    {
        private readonly AppDbContext _context;

        public ContratacaoRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Contratacao>> GetAllAsync()
        {
            return await _context.Contratacoes
                .Include(c => c.Cliente)
                .Include(c => c.Produto)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task AddAsync(Contratacao contratacao)
        {
            await _context.Contratacoes.AddAsync(contratacao);

            await _context.SaveChangesAsync();
        }

        public async Task<Contratacao?> GetByIdAsync(int id)
        {
            return await _context.Contratacoes
                .Include(c => c.Cliente)
                .Include(c => c.Produto)
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task UpdateAsync(Contratacao contratacao)
        {
            _context.Contratacoes.Update(contratacao);

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Contratacao contratacao)
        {
            _context.Contratacoes.Remove(contratacao);

            await _context.SaveChangesAsync();
        }
    }
}