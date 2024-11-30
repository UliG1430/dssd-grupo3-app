using System.Threading.Tasks;
using Backend.Data;
using Backend.Model;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repositories
{
    public class EvaluacionRepository : GenericRepository<Evaluacion>
    {
        private readonly ApiDbContext _context;

        public EvaluacionRepository(ApiDbContext context): base(context)
        {
            _context = context;
        }

        public async Task<Evaluacion> GetByCaseIdAsync(int caseId)
        {
            return await _context.Set<Evaluacion>().FirstOrDefaultAsync(e => e.caseId == caseId);
        }

        public async Task AddEvaluacionAsync(Evaluacion evaluacion)
        {
            await _context.Set<Evaluacion>().AddAsync(evaluacion);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateEvaluacionAsync(Evaluacion evaluacion)
        {
            _context.Set<Evaluacion>().Update(evaluacion);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Evaluacion>> GetAllEvaluacionesWithEnvStateAsync()
        {
            return await _context.Set<Evaluacion>()
                .Where(e => e.state == "ENV")
                .ToListAsync();
        }
    }
}
