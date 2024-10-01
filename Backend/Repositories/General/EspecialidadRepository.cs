using api.Data;
using api.Model;

namespace ApiACEAPP.Repositories
{
    public class EspecialidadRepository : GenericRepository<Especialidad>
    {
        public EspecialidadRepository(ApiDbContext context) : base(context)
        {
        }
    }
}