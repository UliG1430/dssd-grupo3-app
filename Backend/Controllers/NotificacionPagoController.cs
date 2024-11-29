using System.Threading.Tasks;
using Backend.Model;
using Backend.Dto;
using Backend.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class NotificacionPagoController : ControllerBase
    {
        private readonly NotificacionPagoRepository _repository;

        public NotificacionPagoController(NotificacionPagoRepository repository)
        {
            _repository = repository;
        }

        [HttpPost]
        public async Task<IActionResult> AddNotificacionPago([FromBody] NotificacionPagoDTO body)
        {
            try {
                var notificacion = new NotificacionPago
                {
                    caseId = body.CaseId,
                    cantidad = body.Cantidad,
                };

                await _repository.AddAsync(notificacion);

                return Ok(notificacion);
            } catch (System.Exception e) {
                Console.WriteLine(e);
                return BadRequest(e.Message);
            }

        }

        [HttpGet("ByCaseId/{caseId}")]
        public async Task<IActionResult> GetByCaseId(int caseId)
        {
            var notificacion = await _repository.GetByCaseIdAsync(caseId);
            if (notificacion == null)
            {
                return Ok(null);
            }

            return Ok(notificacion);
        }

    }
}
