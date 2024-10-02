using Backend.Data;
using Backend.Model;

namespace Backend.Repositories
{
    public class OrdenRepository : GenericRepository<Orden>
    {
        public OrdenRepository(ApiDbContext context) : base(context)
        {
        }
    }
}