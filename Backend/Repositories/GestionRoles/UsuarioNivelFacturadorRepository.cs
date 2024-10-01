using api.Data;
using api.Model;

namespace ApiACEAPP.Repositories
{
    public class UsuarioNivelFacturadorRepository : GenericRepository<UsuarioNivelFacturador>
    {
        public UsuarioNivelFacturadorRepository(ApiDbContext context) : base(context)
        {
        }
    }
}