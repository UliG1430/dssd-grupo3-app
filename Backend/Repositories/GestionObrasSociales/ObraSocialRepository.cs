using api.Data;
using api.Model;

namespace ApiACEAPP.Repositories
{
    public class ObraSocialRepository : GenericRepository<ObraSocial>
    {
        public ObraSocialRepository(ApiDbContext context) : base(context)
        {
        }

        public async Task<ObraSocial?> GetByName(string nombre)
        {
            return (await FilterAsync(x => x.Nombre == nombre)).FirstOrDefault();
        }
    }
}