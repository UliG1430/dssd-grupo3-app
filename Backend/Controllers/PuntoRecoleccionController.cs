using Backend.Dto;
using Backend.Model;
using Backend.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[ApiController]
[Route("[controller]")]
public class PuntoRecoleccionController : ControllerBase
{
    private readonly PuntoRecoleccionRepository _puntoRecoleccionRepository;

    public PuntoRecoleccionController(PuntoRecoleccionRepository puntoRecoleccionRepository)
    {
        _puntoRecoleccionRepository = puntoRecoleccionRepository;
    }

    [HttpGet("PuntosRecoleccion")]
    //[Authorize]
    public async Task<IActionResult> getPuntosRecoleccion()
    {
        try
        {
            var puntosRecoleccion = await _puntoRecoleccionRepository.GetAllAsync();

            return Ok(puntosRecoleccion);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}