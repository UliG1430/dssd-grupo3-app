using api.Data;
using api.Model;

namespace ApiACEAPP.Repositories
{
    public class ValidacionRepository : GenericRepository<Validacion>
    {
        public ValidacionRepository(ApiDbContext context) : base(context)
        {
        }
    }
}