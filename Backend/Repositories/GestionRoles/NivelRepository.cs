using api.Data;
using api.Model;

namespace ApiACEAPP.Repositories
{
    public class NivelRepository : GenericRepository<Nivel>
    {
        public NivelRepository(ApiDbContext context) : base(context)
        {
        }
    }
}