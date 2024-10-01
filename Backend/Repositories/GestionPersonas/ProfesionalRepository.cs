using api.Data;
using api.Model;

namespace ApiACEAPP.Repositories
{
    public class ProfesionalRepository : GenericRepository<Profesional>
    {
        public ProfesionalRepository(ApiDbContext context) : base(context)
        {
        }
    }
}