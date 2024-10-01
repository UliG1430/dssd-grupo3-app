using api.Data;
using api.Model;

namespace ApiACEAPP.Repositories
{
    public class UsuarioRepository : GenericRepository<Usuario>
    {
        public UsuarioRepository(ApiDbContext context) : base(context)
        {
        }
    }
}