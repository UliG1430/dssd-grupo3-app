using api.Data;
using api.Model;

namespace ApiACEAPP.Repositories
{
    public class DiagnosticoRepository : GenericRepository<Diagnostico>
    {
        public DiagnosticoRepository(ApiDbContext context) : base(context)
        {
        }
    }
}