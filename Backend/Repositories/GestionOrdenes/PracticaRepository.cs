using api.Data;
using api.Model;

namespace ApiACEAPP.Repositories
{
    public class PracticaRepository : GenericRepository<Practica>
    {
        public PracticaRepository(ApiDbContext context) : base(context)
        {
        }
    }
}