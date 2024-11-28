using Backend.Dto;
using Backend.Model;
using Backend.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[ApiController]
[Route("[controller]")]
public class PaqueteController : ControllerBase
{
    private readonly PaqueteRepository _paqueteRepository;

    public PaqueteController(PaqueteRepository paqueteRepository)
    {
        _paqueteRepository = paqueteRepository;
    }

    [HttpPost("AddPaquete")]
    //[Authorize]
    public async Task<IActionResult> AddPaquete(CargarPaqueteDTO body)
    {
        try
        {
            var paquete = new Paquete
            {
                CaseId = body.caseId,
                State = body.state
            };

            await _paqueteRepository.AddAsync(paquete);

            return Ok(paquete);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    // New GetPaquete by CaseId
    [HttpGet("{caseId}")]
    public async Task<IActionResult> GetPaqueteByCaseId(int caseId)
    {
        try
        {
            var paquete = await _paqueteRepository.GetByCaseIdAsync(caseId);

            if (paquete == null)
            {
                return NotFound(new { message = $"Paquete with CaseId {caseId} not found." });
            }

            return Ok(paquete);
        }
        catch (Exception e)
        {
            return BadRequest(new { message = "Error retrieving paquete.", error = e.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePaquete(int id, [FromBody] UpdatePaqueteDTO body)
    {
        try
        {
            // Find the existing Paquete
            var paquete = await _paqueteRepository.GetByIdAsync(id);
            if (paquete == null)
            {
                return NotFound(new { message = $"Paquete with Id {id} not found." });
            }

            // Update the fields
            paquete.State = body.state ?? paquete.State; // Update only if provided

            // Save changes
            _paqueteRepository.Update(paquete);
            await _paqueteRepository.SaveChangesAsync();

            return Ok(new { message = "Paquete updated successfully", paquete });
        }
        catch (Exception e)
        {
            return BadRequest(new { message = "Error updating paquete.", error = e.Message });
        }
    }

    // New Retrieve Paquetes by State
    [HttpGet("byState/{state}")]
    public async Task<IActionResult> GetPaquetesByState(string state)
    {
        try
        {
            var paquetes = await _paqueteRepository.GetByStateAsync(state);

            if (paquetes == null || paquetes.Count == 0)
            {
                return Ok(new List<Paquete>());
            }

            return Ok(paquetes);
        }
        catch (Exception e)
        {
            return BadRequest(new { message = "Error retrieving paquetes by state.", error = e.Message });
        }
    }
}