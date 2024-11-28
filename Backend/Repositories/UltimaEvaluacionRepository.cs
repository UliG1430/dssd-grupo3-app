using Backend.Data;
using Backend.Model;

namespace Backend.Repositories
{
    public class UltimaEvaluacionRepository : GenericRepository<UltimaEvaluacion>
    {
        public UltimaEvaluacionRepository(ApiDbContext context) : base(context)
        {
        }
    }
}