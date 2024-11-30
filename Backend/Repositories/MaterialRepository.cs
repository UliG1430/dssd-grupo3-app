using Backend.Data;
using Backend.Model;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

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

         public async Task<double?> GetStockByCodeAsync(string codMaterial)
         {

                // Obtiene el stock del material filtrando por su código
                return await _context.Materiales
                    .Where(m => m.CodMaterial == codMaterial) // Filtra por código del material
                    .Select(m => (double?)m.StockActual)      // Selecciona el stock como double?
                    .SingleOrDefaultAsync();                  // Asegura que solo haya un resultado o null si no existe
            }
         }
    }