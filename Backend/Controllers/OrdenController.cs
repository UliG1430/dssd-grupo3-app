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
                PuntoRecoleccion = body.Zona,
                Fecha = DateTime.UtcNow,
                CaseId = body.CaseId,
                UsuarioId = body.UsuarioId
            };

            await _ordenRepository.AddAsync(orden);

            return Ok(orden);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}