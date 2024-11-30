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

        public async Task<IEnumerable<Orden>> GetOrdenesForUsuarioInLastTwoWeeksAsync(int idUsuario)
        {
            var twoWeeksAgo = DateTime.UtcNow.AddDays(-14); // Use UTC
            var now = DateTime.UtcNow;

            return await _context.Set<Orden>()
                .Where(o => o.UsuarioId == idUsuario && o.Fecha >= twoWeeksAgo && o.Fecha <= now)
                .ToListAsync();
        }

        public async Task<List<RecolectoresMasCargan>> GetRecolectoresMasCargan(DateTime fechaInicio, DateTime fechaFin, int cant)
        {
            // SELECT o.UsuarioId, COUNT(*) as CantidadOrdenes
            // FROM Ordenes o
            // WHERE o.Fecha >= fechaInicio AND o.Fecha <= fechaFin AND o.estado = 'OK'
            // GROUP BY o.UsuarioId
            // ORDER BY CantidadOrdenes DESC

            List<RecolectoresMasCargan> result = await _dbContext.Ordenes
                .Where(o => o.Fecha >= fechaInicio && o.Fecha <= fechaFin && o.estado == "OK")
                .GroupBy(o => o.UsuarioId)
                .Select(g => new RecolectoresMasCargan
                {
                    RecolectorId = g.Key,
                    CantidadOrdenes = g.Count()
                })
                .OrderByDescending(g => g.CantidadOrdenes)
                .ToListAsync();

            result = result.Take(cant).ToList();

            return result;
        }

        public async Task<List<ProporcionDiscrepancias>> GetProporcionDiscrepancias()
        {
            // SELECT o.UsuarioId, COUNT(*) as CantidadOrdenes, 
            //     SUM(CASE WHEN o.estado = 'INV' THEN 1 ELSE 0 END) as CantidadDiscrepancias,
            //     (SUM(CASE WHEN o.estado = 'INV' THEN 1 ELSE 0 END) / COUNT(*)) * 100 as Proporcion
            // FROM Ordenes o
            // GROUP BY o.UsuarioId

            var result = await _dbContext.Ordenes
                .GroupBy(o => o.UsuarioId)
                .Select(g => new ProporcionDiscrepancias
                {
                    UsuarioId = g.Key,
                    CantidadOrdenes = g.Count(),
                    CantidadDiscrepancias = g.Count(o => o.estado == "INV"),
                    Proporcion = ((double)g.Count(o => o.estado == "INV") / g.Count() * 100).ToString("0.00") + "%"
                })
                .ToListAsync();
            return result;
        }
        
        public async Task<List<ProveedoresMasEficientesResult>> GetProveedoresMasEficientes(DateTime fechaInicio, DateTime fechaFin, int cant)
        {
            // SELECT o.PuntoRecoleccionId, AVG(CASE WHEN o.estado != 'ENV' THEN DATEDIFF(MINUTE, o.Fecha, o.FechaCambioEstado) END) as TiempoPromedio,
            //     (SUM(CASE WHEN o.estado != 'ENV' THEN 1 ELSE 0 END) / COUNT(*)) * 100 as ProporcionVerificada
            // FROM Ordenes o
            // WHERE o.Fecha >= fechaInicio AND o.Fecha <= fechaFin
            // GROUP BY o.PuntoRecoleccionId
            // ORDER BY ProporcionVerificada DESC, TiempoPromedio ASC

            List<ProveedoresMasEficientesResult> result = await _dbContext.Ordenes
                .Where(o => o.Fecha >= fechaInicio && o.Fecha <= fechaFin)
                .GroupBy(o => o.PuntoRecoleccionId)
                .Select(g => new ProveedoresMasEficientesResult
                {
                    PuntoRecoleccionId = g.Key,
                    TiempoPromedio = g.Average(o => o.estado != "ENV" ? (o.FechaCambioEstado - o.Fecha).TotalMinutes : 0),
                    ProporcionVerificadas = ((double)g.Count(o => o.estado != "ENV") / g.Count() * 100).ToString("0.00") + "%"
                })
                .OrderBy(g => g.TiempoPromedio)
                .ToListAsync();

            result = result.Take(cant).ToList();
            
            return result;
        }

        public async Task<List<TiempoPromedioPorMaterial>> GetTiempoPromedioDeProcesamientoPorMaterial()
        {
            // SELECT m.Nombre, AVG(DATEDIFF(MINUTE, o.Fecha, o.FechaCambioEstado)) as TiempoPromedioDeProcesamiento
            // FROM Ordenes o INNER JOIN Materiales m ON o.Material = m.Id
            // WHERE o.estado != 'ENV'
            // GROUP BY m.Nombre
            
            List<TiempoPromedioPorMaterial> result = await _dbContext.Ordenes
                .Join(_dbContext.Materiales, o => o.Material, m => m.Id.ToString(), (o, m) => new { o, m })
                .Where(o => o.o.estado != "ENV")
                .GroupBy(o => o.m.Nombre)
                .Select(g => new TiempoPromedioPorMaterial
                {
                    NombreMaterial = g.Key,
                    TiempoPromedioDeProcesamiento = g.Average(o => (o.o.FechaCambioEstado - o.o.Fecha).TotalMinutes).ToString("0.00") + " minutos"
                })
                .ToListAsync();
            
            return result;
        }
    }

    //DTO
    public class RecolectoresMasCargan
    {
        public int RecolectorId { get; set; }
        public int CantidadOrdenes { get; set; }
    }

    public class ProporcionDiscrepancias
    {
        public int UsuarioId { get; set; }
        public int CantidadOrdenes { get; set; }
        public int CantidadDiscrepancias { get; set; }
        public string Proporcion { get; set; }
    }

    public class ProveedoresMasEficientesResult
    {
        public int PuntoRecoleccionId { get; set; }
        public double TiempoPromedio { get; set; }
        public string ProporcionVerificadas { get; set; }
    }

    public class TiempoPromedioPorMaterial
    {
        public string NombreMaterial { get; set; }
        public string TiempoPromedioDeProcesamiento { get; set; }
    }
}