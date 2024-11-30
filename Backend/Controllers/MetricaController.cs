using Backend.Dto;
using Backend.Repositories;
using Backend.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;


namespace api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MetricaController : ControllerBase
    {
        private readonly UsuarioRepository _usuarioRepository;
        private readonly OrdenRepository _ordenRepository;
        private readonly PuntoRecoleccionRepository _puntoRecoleccionRepository;

        public MetricaController(UsuarioRepository usuarioRepository, 
                                 OrdenRepository ordenRepository,
                                 PuntoRecoleccionRepository puntoRecoleccionRepository)
        {
            _usuarioRepository = usuarioRepository;
            _ordenRepository = ordenRepository;
            _puntoRecoleccionRepository = puntoRecoleccionRepository;
        }

        [HttpPost("GetRecolectoresMasCargan")]
        public async Task<IActionResult> GetRecolectoresMasCargan([FromBody] MetricaInicioFinCantidad body)
        {
            try
            {
                List<RecolectoresMasCargan> recolectoresMasCargan = await _ordenRepository.GetRecolectoresMasCargan(body.FechaInicio, body.FechaFin, body.Cantidad);

                var result = new List<object>();
                recolectoresMasCargan.ForEach(m => result.Add(new 
                {
                    Recolector = _usuarioRepository.GetByIdAsync(m.RecolectorId).Result.UsuarioNombre,
                    CantidadOrdenesBienCargadas = m.CantidadOrdenes
                }));
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        
        [HttpGet("GetPromedioDiscrepancias")]
        public async Task<IActionResult> GetPromedioDiscrepancias()
        {
            try
            {
                List<ProporcionDiscrepancias> proporcionDiscrepancias = await _ordenRepository.GetProporcionDiscrepancias();

                var result = new List<object>();
                proporcionDiscrepancias.ForEach(p => result.Add(new 
                {
                    Usuario = _usuarioRepository.GetAsync(p.UsuarioId).Result.UsuarioNombre,
                    CantidadOrdenes = p.CantidadOrdenes,
                    CantidadDiscrepancias = p.CantidadDiscrepancias,
                    Proporcion = p.Proporcion
                }));

                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        
        [HttpPost("GetProveedoresMasEficientes")]
        public async Task<IActionResult> GetProveedoresMasEficientes([FromBody] MetricaInicioFinCantidad body)
        {
            try
            {
                List<ProveedoresMasEficientesResult> proveedoresMasEficientes = await _ordenRepository.GetProveedoresMasEficientes(body.FechaInicio, body.FechaFin, body.Cantidad);

                var result = new List<object>();
                proveedoresMasEficientes.ForEach(m => result.Add(new 
                {
                    Proveedor = _puntoRecoleccionRepository.GetAsync(m.PuntoRecoleccionId).Result,
                    TiempoPromedioDeVerificacion = m.TiempoPromedio.ToString("0.00") + " minutos",
                    ProporcionOrdenesVerificadas = m.ProporcionVerificadas
                }));

                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("GetTiempoPromedioDeProcesamientoPorMaterial")]
        public async Task<IActionResult> GetTiempoPromedioDeProcesamientoPorMaterial()
        {
            try
            {
                var result = await _ordenRepository.GetTiempoPromedioDeProcesamientoPorMaterial();
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
    // DTOs
    public class MetricaInicioFinCantidad
    {
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public int Cantidad { get; set; }
    }
}
