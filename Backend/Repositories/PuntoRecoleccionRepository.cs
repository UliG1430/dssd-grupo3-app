using Backend.Data;
using Backend.Model;

namespace Backend.Repositories
{
    public class PuntoRecoleccionRepository : GenericRepository<PuntoRecoleccion>
    {
        public PuntoRecoleccionRepository(ApiDbContext context) : base(context)
        {
        }
    }
}