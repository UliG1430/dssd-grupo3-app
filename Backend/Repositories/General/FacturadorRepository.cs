using api.Data;
using api.Model;

namespace ApiACEAPP.Repositories
{
    public class FacturadorRepository : GenericRepository<Facturador>
    {
        public FacturadorRepository(ApiDbContext context) : base(context)
        {
        }
    }
}