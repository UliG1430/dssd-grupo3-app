using api.Data;
using api.Model;

namespace ApiACEAPP.Repositories
{
    public class SolicitudRepository : GenericRepository<Solicitud>
    {
        public SolicitudRepository(ApiDbContext context) : base(context)
        {
        }
    }
}