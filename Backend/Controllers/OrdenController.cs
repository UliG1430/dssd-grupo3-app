using Backend.Dto;
using Backend.Model;
using Backend.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[ApiController]
[Route("[controller]")]
public class OrdenController : ControllerBase
{
    private readonly OrdenRepository _ordenRepository;

    public OrdenController(OrdenRepository ordenRepository)
    {
        _ordenRepository = ordenRepository;
    }

    [HttpPost("AddOrden")]
    //[Authorize]
    public async Task<IActionResult> AddOrden(CargarOrdenForm body)
    {
        try
        {
            var orden = new Orden
            {
                Material = body.Material,
                PesoKg = body.Cantidad,
                PuntoRecoleccionId = body.Zona,
                Fecha = DateTime.UtcNow,
                CaseId = body.CaseId,
                UsuarioId = body.UsuarioId,
                paqueteId = body.PaqueteId,
                revisado = body.Revisado,
                estado = body.Estado,
                FechaCambioEstado = DateTime.UtcNow
            };

            await _ordenRepository.AddAsync(orden);

            return Ok(orden);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    // Get Ordenes by Paquete Id
    [HttpGet("ByPaquete/{paqueteId}")]
    public async Task<IActionResult> GetOrdenesByPaqueteId(int paqueteId)
    {
        try
        {
            var ordenes = await _ordenRepository.GetByPaqueteIdAsync(paqueteId);

            if (ordenes == null)
            {
                return Ok(new List<Orden>());
            }

            return Ok(ordenes);
        }
        catch (Exception e)
        {
            return BadRequest(new { message = "Error retrieving ordenes.", error = e.Message });
        }
    }

    // New Update Orden State
    [HttpPut("UpdateState/{id}")]
    public async Task<IActionResult> UpdateOrdenState(int id, [FromBody] UpdateOrdenStateDto body)
    {
        try
        {
            // Find the existing Orden
            var orden = await _ordenRepository.GetByIdAsync(id);
            if (orden == null)
            {
                return NotFound(new { message = $"Orden with Id {id} not found." });
            }

            // Update the State field
            orden.estado = body.Estado;
            orden.FechaCambioEstado = DateTime.UtcNow;

            // Save changes
            _ordenRepository.Update(orden);
            await _ordenRepository.SaveChangesAsync();

            return Ok(new { message = "Orden state updated successfully", orden });
        }
        catch (Exception e)
        {
            return BadRequest(new { message = "Error updating orden state.", error = e.Message });
        }
    }

    [HttpGet("GetOrdenesForUsuario/{idUsuario}")]
    public async Task<IActionResult> GetOrdenesForUsuarioInLastTwoWeeks(int idUsuario)
    {
        try
        {
            var ordenes = await _ordenRepository.GetOrdenesForUsuarioInLastTwoWeeksAsync(idUsuario);

            if (ordenes == null || !ordenes.Any())
            {
                return Ok(new List<Orden>());
            }

            return Ok(ordenes);
        }
        catch (Exception e)
        {
            return BadRequest(new { message = "Error retrieving ordenes.", error = e.Message });
        }
    }


}