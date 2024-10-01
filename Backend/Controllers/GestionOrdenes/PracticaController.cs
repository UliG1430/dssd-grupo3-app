using api.Dto;
using api.Model;
using ApiACEAPP.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[ApiController]
[Route("[controller]")]
public class PracticaController : ControllerBase
{
    private readonly ILogger<PracticaController> _logger;
    private readonly PracticaRepository _practicaRepository;
    public PracticaController(ILogger<PracticaController> logger,
                              PracticaRepository practicaRepository)
    {
        _logger = logger;
        _practicaRepository = practicaRepository;
    }

    [HttpGet("GetPracticas")]
    [Authorize]
    public async Task<IActionResult> GetPracticas()
    {
        try
        {
            IEnumerable<Practica> practicas = await _practicaRepository.FilterAsync(x => x.Activa == true);
            return Ok(practicas);
        }
        catch (Exception ex)
        {
            //_logger.LogError(ex, "Error al obtener las practicas");
            return StatusCode(500, "Error al obtener las practicas");
        }
    }

    [HttpPost("AgregarPractica")]
    public async Task<IActionResult> AgregarPractica(PracticaModel body)
    {
        try
        {
            var practica = (await _practicaRepository.FilterAsync(x => x.Codigo == body.codigo)).FirstOrDefault();

            if (practica != null)
            {
                // el codigo ya esta en uso
                return Ok(false);
            }
            else
            {
                var practicaNueva = new Practica()
                {
                    Id = Guid.NewGuid(),
                    Codigo = body.codigo,
                    Descripcion = body.descripcion,
                    Tipo = body.tipo,
                    Activa = body.activa,
                    Monto = body.monto,
                };

                var practicaCreada = await _practicaRepository.AddAsync(practicaNueva);

                return Ok(true);
            }
        }
        catch (Exception e)
        {
            //_logger.LogError(e, "Error al agregar la practica");
            return StatusCode(500, "Error al agregar la practica");
        }
    }
}