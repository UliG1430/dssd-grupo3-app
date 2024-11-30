using Microsoft.EntityFrameworkCore;

using Backend.Data;
using Backend.Model;

namespace Backend.Repositories
{
    public class OrdenRepository : GenericRepository<Orden>
    {

        private readonly ApiDbContext _dbContext;

        public OrdenRepository(ApiDbContext context) : base(context)
        {
            _dbContext = context;
        }

        public async Task<List<Orden>> GetByPaqueteIdAsync(int paqueteId)
        {
            return await _dbContext.Ordenes
                .Where(o => o.paqueteId == paqueteId && o.estado == "ENV")
                .Include(o => o.PuntoRecoleccion) // Include PuntoRecoleccion for eager loading
                .ToListAsync();
        }

        public async Task<Orden> GetByIdAsync(int id)
        {
            return await _dbContext.Ordenes.FindAsync(id);
        }

        public void Update(Orden orden)
        {
            _dbContext.Ordenes.Update(orden);
        }

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<MaxOrdenesRepositoresReturn>> GetMaxOrdenesRecolectores(DateTime fechaInicio, DateTime fechaFin, int cant)
        {
            List<MaxOrdenesRepositoresReturn> result = await _dbContext.Ordenes
                .Where(o => o.Fecha >= fechaInicio && o.Fecha <= fechaFin && o.estado == "OK")
                .GroupBy(o => o.UsuarioId)
                .Select(g => new MaxOrdenesRepositoresReturn
                {
                    RecolectorId = g.Key,
                    CantidadOrdenes = g.Count()
                })
                .OrderByDescending(g => g.CantidadOrdenes)
                .ToListAsync();

            result = result.Take(cant).ToList();

            return result;
        }
        
        public async Task<List<RecolectoresMaxOrdenesReturn>> GetProveedoresMaxPedidosCompletados(DateTime fechaInicio, DateTime fechaFin, int cant)
        {
            List<RecolectoresMaxOrdenesReturn> result = await _dbContext.Ordenes
                .Where(o => o.Fecha >= fechaInicio && o.Fecha <= fechaFin)// && o.estado == "OK"
                .GroupBy(o => o.PuntoRecoleccionId)
                .Select(g => new RecolectoresMaxOrdenesReturn
                {
                    PuntoRecoleccionId = g.Key,
                    CantidadPedidos = g.Count()
                })
                .OrderByDescending(g => g.CantidadPedidos)
                .ToListAsync();

            result = result.Take(cant).ToList();
            
            return result;

        }
    }

    //DTO
    public class MaxOrdenesRepositoresReturn
    {
        public int RecolectorId { get; set; }
        public int CantidadOrdenes { get; set; }
    }

    public class RecolectoresMaxOrdenesReturn
    {
        public int PuntoRecoleccionId { get; set; }
        public int CantidadPedidos { get; set; }
    }
}