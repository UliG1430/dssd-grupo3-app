using System.Threading.Tasks;
using Backend.Dto;
using Backend.Model;
using Backend.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class EvaluacionController : ControllerBase
    {
        private readonly EvaluacionRepository _repository;

        public EvaluacionController(EvaluacionRepository repository)
        {
            _repository = repository;
        }

        // AddEvaluacion/<caseId>
        [HttpPost("AddEvaluacion/{caseId}")]
        public async Task<IActionResult> AddEvaluacion(int caseId)
        {
            try
            {
                // Initialize new Evaluacion
                var newEvaluacion = new Evaluacion
                {
                    caseId = caseId,
                    state = "ENV",
                    observaciones = "",
                    cantOrdenes = 0,
                    cantOrdenesOk = 0,
                    cantOrdenesMal = 0
                };

                await _repository.AddEvaluacionAsync(newEvaluacion);
                return CreatedAtAction(nameof(GetEvaluacion), new { caseId = caseId }, newEvaluacion);
            }
            catch (Exception e)
            {
                return BadRequest(new { message = "Error adding Evaluacion.", error = e.Message });
            }
        }

        // UpdateEvaluacion
        [HttpPost("UpdateEvaluacion")]
        public async Task<IActionResult> UpdateEvaluacion([FromBody] UpdateEvaluacionDTO updatedEvaluacion)
        {
            try
            {
                // Ensure the Evaluacion exists
                var existingEvaluacion = await _repository.GetByCaseIdAsync(updatedEvaluacion.caseId);
                if (existingEvaluacion == null)
                {
                    return NotFound(new { message = $"Evaluacion with caseId {updatedEvaluacion.caseId} not found." });
                }

                // Update the Evaluacion (except the ID)
                existingEvaluacion.state = updatedEvaluacion.state;
                existingEvaluacion.observaciones = updatedEvaluacion.observaciones;
                existingEvaluacion.cantOrdenes = updatedEvaluacion.cantOrdenes;
                existingEvaluacion.cantOrdenesOk = updatedEvaluacion.cantOrdenesOk;
                existingEvaluacion.cantOrdenesMal = updatedEvaluacion.cantOrdenesMal;

                await _repository.UpdateEvaluacionAsync(existingEvaluacion);
                return Ok(new { message = "Evaluacion updated successfully.", existingEvaluacion });
            }
            catch (Exception e)
            {
                return BadRequest(new { message = "Error updating Evaluacion.", error = e.Message });
            }
        }

        // GetEvaluacion/<caseId>
        [HttpGet("GetEvaluacion/{caseId}")]
        public async Task<IActionResult> GetEvaluacion(int caseId)
        {
            try
            {
                var evaluacion = await _repository.GetByCaseIdAsync(caseId);
                if (evaluacion == null)
                {
                    return NotFound(new { message = $"Evaluacion with caseId {caseId} not found." });
                }

                return Ok(evaluacion);
            }
            catch (Exception e)
            {
                return BadRequest(new { message = "Error retrieving Evaluacion.", error = e.Message });
            }
        }

        [HttpGet("GetAllWithEnvState")]
        public async Task<IActionResult> GetAllEvaluacionesWithEnvState()
        {
            try
            {
                var evaluaciones = await _repository.GetAllEvaluacionesWithEnvStateAsync();
                if (evaluaciones == null || !evaluaciones.Any())
                {
                    return Ok(new List<Evaluacion>());
                }

                return Ok(evaluaciones);
            }
            catch (Exception e)
            {
                return BadRequest(new { message = "Error retrieving evaluaciones.", error = e.Message });
            }
        }

    }
}
