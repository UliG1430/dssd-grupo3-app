using Backend.Data;
using Backend.Model;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Backend.Repositories
{
    public class MaterialRepository : GenericRepository<Material>
    {
        public MaterialRepository(ApiDbContext context) : base(context)
        {
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