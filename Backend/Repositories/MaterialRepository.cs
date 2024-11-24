using Backend.Data;
using Backend.Model;

namespace Backend.Repositories
{
    public class MaterialRepository : GenericRepository<Material>
    {
        public MaterialRepository(ApiDbContext context) : base(context)
        {
        }
    }
}