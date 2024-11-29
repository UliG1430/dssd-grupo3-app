using Backend.Data;
using Backend.Model;

namespace Backend.Repositories
{
    public class MaterialRepository : GenericRepository<Material>
    {

        private readonly ApiDbContext _context;

        public MaterialRepository(ApiDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Material> GetByIdAsync(int id)
        {
            return await _context.Materiales.FindAsync(id);
        }

        public void Update(Material material)
        {
            _context.Materiales.Update(material);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}