using Microsoft.EntityFrameworkCore;

using Backend.Data;
using Backend.Model;

namespace Backend.Repositories
{
    public class PaqueteRepository : GenericRepository<Paquete>
    {
        private readonly ApiDbContext _dbContext;

        public PaqueteRepository(ApiDbContext context) : base(context)
        {
            _dbContext = context;
        }

        public async Task<Paquete> GetByCaseIdAsync(int caseId)
        {
            return await _dbContext.Paquetes
                .FirstOrDefaultAsync(p => p.CaseId == caseId);
        }

        public async Task<Paquete> GetByIdAsync(int id)
        {
            return await _dbContext.Paquetes.FindAsync(id);
        }

        public void Update(Paquete paquete)
        {
            _dbContext.Paquetes.Update(paquete);
        }

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<Paquete>> GetByStateAsync(string state)
        {
            return await _dbContext.Paquetes
                .Where(p => p.State == state)
                .ToListAsync();
        }

    }
}