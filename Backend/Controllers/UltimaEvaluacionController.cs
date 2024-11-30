using System.Threading.Tasks;
using Backend.Model;
using Backend.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UltimaEvaluacionController : ControllerBase
    {
        private readonly UltimaEvaluacionRepository _repository;

        public UltimaEvaluacionController(UltimaEvaluacionRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetUltimaEvaluacion()
        {
            var ultimaEvaluacion = await _repository.GetUltimaEvaluacionAsync();
            if (ultimaEvaluacion == null)
            {
                return NotFound(new { message = "No UltimaEvaluacion found." });
            }

            return Ok(ultimaEvaluacion);
        }

        [HttpPut("SetFechaToNow")]
        public async Task<IActionResult> SetUltimaEvaluacionFechaToNow()
        {
            try
            {
                await _repository.SetUltimaEvaluacionFechaToNowAsync();
                return Ok(new { message = "Fecha of UltimaEvaluacion updated to current date and time." });
            }
            catch (Exception e)
            {
                return BadRequest(new { message = "Error updating Fecha of UltimaEvaluacion.", error = e.Message });
            }
        }
    }
}
