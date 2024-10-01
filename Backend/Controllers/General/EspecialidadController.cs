using api.Dto;
using api.Model;
using ApiACEAPP.Repositories;
using ApiACEAPP.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[ApiController]
[Route("[controller]")]
public class EspecialidadController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<PacienteController> _logger;
    private readonly EspecialidadRepository _especialidadRepository;

    public EspecialidadController(IConfiguration configuration, ILogger<PacienteController> logger,
                                  EspecialidadRepository especialidadRepository)
    {
        _configuration = configuration;
        _logger = logger;
        _especialidadRepository = especialidadRepository;
    }

    [HttpPost("AgregarEspecialidad")]
    public async Task<IActionResult> AgregarEspecialidad(AgregarEspecialidadModel body)
    {
        try
        {
            Especialidad? especialidad = (await _especialidadRepository.FilterAsync(x => x.Codigo == body.Codigo)).FirstOrDefault();

            if (especialidad != null)
            {
                // el nombre ya esta en uso
                return Ok(false);
            }
            else
            {
                especialidad = new Especialidad()
                {
                    Id = Guid.NewGuid(),
                    Codigo = body.Codigo,
                    Nombre = body.Nombre,
                    TipoEspecialidad = body.TipoEspecialidad,
                    IdProfesionIOMA = body.IdProfesionIOMA,
                    Vigente = body.Vigente
                };
                
                Especialidad especialidadCreada = await _especialidadRepository.AddAsync(especialidad);
                
                return Ok(especialidadCreada);
            }
        }
        catch (Exception ex)
        {
            //_logger.LogError(ex, "Error al agregar especialidad");
            return BadRequest(ex);
        }
    }
}