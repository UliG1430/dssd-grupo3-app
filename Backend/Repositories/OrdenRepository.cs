using Microsoft.EntityFrameworkCore;

using Backend.Data;
using Backend.Model;

namespace Backend.Repositories
{
    public class OrdenRepository : GenericRepository<Orden>
    {

        private readonly ApiDbContext _dbContext;

        public OrdenRepository(ApiDbContext context) : base(context)
        {
            _dbContext = context;
        }

        public async Task<List<Orden>> GetByPaqueteIdAsync(int paqueteId)
        {
            return await _dbContext.Ordenes
                .Where(o => o.paqueteId == paqueteId && o.estado == "ENV")
                .Include(o => o.PuntoRecoleccion) // Include PuntoRecoleccion for eager loading
                .ToListAsync();
        }

        public async Task<Orden> GetByIdAsync(int id)
        {
            return await _dbContext.Ordenes.FindAsync(id);
        }

        public void Update(Orden orden)
        {
            _dbContext.Ordenes.Update(orden);
        }

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Orden>> GetOrdenesForUsuarioInLastTwoWeeksAsync(int idUsuario)
        {
            var twoWeeksAgo = DateTime.UtcNow.AddDays(-14); // Use UTC
            var now = DateTime.UtcNow;

            return await _context.Set<Orden>()
                .Where(o => o.UsuarioId == idUsuario && o.Fecha >= twoWeeksAgo && o.Fecha <= now)
                .ToListAsync();
        }

    }
}