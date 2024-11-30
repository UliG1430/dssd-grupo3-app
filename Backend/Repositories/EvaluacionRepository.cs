using Microsoft.EntityFrameworkCore;

using Backend.Data;
using Backend.Model;

namespace Backend.Repositories
{
    public class EvaluacionRepository : GenericRepository<Evaluacion>
    {

        private readonly ApiDbContext _dbContext;

        public EvaluacionRepository(ApiDbContext context) : base(context)
        {
            _dbContext = context;
        }

        public async Task<Evaluacion> GetByIdAsync(int id)
        {
            return await _dbContext.Evaluacion.FindAsync(id);
        }

        public void Update(Evaluacion evaluacion)
        {
            _dbContext.Evaluacion.Update(evaluacion);
        }

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        public async Task<double> GetPromedioDiscrepancias()
        {
            var pares = await _dbContext.Evaluacion
                .Select(e => new { e.cantOrdenes, e.cantOrdenesMal })
                .ToListAsync();
            
            int sumaTotales = 0; int sumaMal = 0;
            foreach (var par in pares)
            {
                sumaTotales += par.cantOrdenes;
                sumaMal += par.cantOrdenesMal;
            }

            return sumaTotales != 0 ? (double)sumaMal / sumaTotales : 0;
        }
    }

    //DTO

}