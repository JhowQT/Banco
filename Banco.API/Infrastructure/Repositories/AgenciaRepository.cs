using Banco.API.Domain.Entities;
using Banco.API.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Banco.API.Infrastructure.Repositories
{
    public class AgenciaRepository
    {
        private readonly AppDbContext _context;

        public AgenciaRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Agencia>> GetAllAsync()
        {
            return await _context.Agencias
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Agencia?> GetByIdAsync(int id)
        {
            return await _context.Agencias
                .Include(a => a.Clientes)
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task AddAsync(Agencia agencia)
        {
            await _context.Agencias.AddAsync(agencia);

            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Agencia agencia)
        {
            _context.Agencias.Update(agencia);

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Agencia agencia)
        {
            _context.Agencias.Remove(agencia);

            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            var agencia = await _context.Agencias.FindAsync(id);

            return agencia != null;
        }
    }
}