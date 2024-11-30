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
        private readonly EvaluacionRepository _evaluacionRepository;
        private readonly PuntoRecoleccionRepository _puntoRecoleccionRepository;

        public MetricaController(UsuarioRepository usuarioRepository, 
                                 OrdenRepository ordenRepository,
                                 EvaluacionRepository evaluacionRepository,
                                 PuntoRecoleccionRepository puntoRecoleccionRepository)
        {
            _usuarioRepository = usuarioRepository;
            _ordenRepository = ordenRepository;
            _evaluacionRepository = evaluacionRepository;
            _puntoRecoleccionRepository = puntoRecoleccionRepository;
        }

        [HttpPost("RecolectoresMaxOrdenes")]
        public async Task<IActionResult> GetRecolectoresMaxOrdenes([FromBody] MetricaInicioFinCantidad body)
        {
            try
            {
                List<MaxOrdenesRepositoresReturn> maxOrdenesRepositores = await _ordenRepository.GetMaxOrdenesRecolectores(body.FechaInicio, body.FechaFin, body.Cantidad);

                var result = new List<object>();
                maxOrdenesRepositores.ForEach(m => result.Add(new 
                {
                    Recolector = _usuarioRepository.GetByIdAsync(m.RecolectorId).Result,
                    CantidadOrdenes = m.CantidadOrdenes
                }));
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        
        [HttpGet("PromedioDiscrepancias")]
        public async Task<IActionResult> GetPromedioDiscrepancias()
        {
            try
            {
                var result = await _evaluacionRepository.GetPromedioDiscrepancias();
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        
        [HttpPost("ProveedoresMaxPedidosCompletados")]
        public async Task<IActionResult> GetProveedoresMaxPedidosCompletados([FromBody] MetricaInicioFinCantidad body)
        {
            try
            {
                List<RecolectoresMaxOrdenesReturn> maxOrdenesRepositores = await _ordenRepository.GetProveedoresMaxPedidosCompletados(body.FechaInicio, body.FechaFin, body.Cantidad);

                var result = new List<object>();
                maxOrdenesRepositores.ForEach(m => result.Add(new 
                {
                    PuntoRecoleccion = _puntoRecoleccionRepository.GetAsync(m.PuntoRecoleccionId).Result,
                    CantidadPedidos = m.CantidadPedidos
                }));
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
