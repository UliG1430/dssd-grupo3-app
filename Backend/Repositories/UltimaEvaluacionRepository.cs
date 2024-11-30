using Backend.Data;
using Backend.Model;
using Microsoft.EntityFrameworkCore;


namespace Backend.Repositories
{
    public class UltimaEvaluacionRepository : GenericRepository<UltimaEvaluacion>
    {

        private readonly ApiDbContext _context;

        public UltimaEvaluacionRepository(ApiDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<UltimaEvaluacion> GetUltimaEvaluacionAsync()
        {
            return await _context.Set<UltimaEvaluacion>().FirstOrDefaultAsync();
        }

        public async Task SetUltimaEvaluacionFechaToNowAsync()
        {
            var ultimaEvaluacion = await _context.Set<UltimaEvaluacion>().FirstOrDefaultAsync();
            if (ultimaEvaluacion != null)
            {
                ultimaEvaluacion.Fecha = DateTime.UtcNow; // Set the date to now
                _context.Set<UltimaEvaluacion>().Update(ultimaEvaluacion);
                await _context.SaveChangesAsync();
            }
        }


    }
}