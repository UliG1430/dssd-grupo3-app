using api.Data;
using api.Model;

namespace ApiACEAPP.Repositories
{
    public class ProfesionalFacturadorRepository : GenericRepository<ProfesionalFacturador>
    {
        public ProfesionalFacturadorRepository(ApiDbContext context) : base(context)
        {
        }
    }
}