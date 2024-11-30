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

        public MetricaController(UsuarioRepository usuarioRepository, 
                                 OrdenRepository ordenRepository)
        {
            _usuarioRepository = usuarioRepository;
            _ordenRepository = ordenRepository;
        }

        [HttpPost("maxOrdenesRecolectores")]
        public async Task<IActionResult> GetMaxOrdenesRecolectores([FromBody] MaxOrdenesRepositoresDto body)
        {
            try
            {
                List<MaxOrdenesRepositoresReturn> maxOrdenesRepositores = _ordenRepository.GetMaxOrdenesRecolectores(body.FechaInicio, body.FechaFin, body.Cantidad);

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
    }
    // DTOs
    public class MaxOrdenesRepositoresDto
    {
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public int Cantidad { get; set; }
    }
}
