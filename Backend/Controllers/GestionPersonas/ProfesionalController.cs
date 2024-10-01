using api.Dto;
using api.Model;
using ApiACEAPP.Repositories;
using ApiACEAPP.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[ApiController]
[Route("[controller]")]
public class ProfesionalController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<PacienteController> _logger;
    private readonly ProfesionalRepository _profesionalRepository;
    private readonly FacturadorRepository _facturadorRepository;
    private readonly ProfesionalFacturadorRepository _profesionalFacturadorRepository;
    private readonly EspecialidadRepository _especialidadRepository;

    public ProfesionalController(IConfiguration configuration, ILogger<PacienteController> logger,
                                 ProfesionalRepository profesionalRepository,
                                 FacturadorRepository facturadorRepository,
                                 ProfesionalFacturadorRepository profesionalFacturadorRepository,
                                 EspecialidadRepository especialidadRepository)
    {
        _configuration = configuration;
        _logger = logger;
        _profesionalRepository = profesionalRepository;
        _facturadorRepository = facturadorRepository;
        _profesionalFacturadorRepository = profesionalFacturadorRepository;
        _especialidadRepository = especialidadRepository;
    }
    
    [HttpPost("AgregarProfesional")]
    public async Task<IActionResult> AgregarProfesional(ProfesionalModel body)
    {
        try
        {
            //Cuit o matrícula?
            var profesional = (await _profesionalRepository.FilterAsync(x => x.Cuit == body.cuit)).FirstOrDefault();

            if (profesional != null)
            {
                // el cuit ya esta en uso
                return Ok(false);
            }
            else
            {
                var profesionalNuevo = new Profesional()
                {
                    Id = Guid.NewGuid(),
                    Cuit = body.cuit,
                    MatriculaProvincial = body.matriculaProvincial,
                    MatriculaNacional = body.matriculaNacional,
                    Apellidos = body.apellidos,
                    Nombres = body.nombres,
                    Especialidades = new List<ProfesionalEspecialidad>(),
                    Domicilio = body.domicilio,
                    Telefono = body.telefono,
                    Email = body.email,
                    FechaNacimiento = body.fechaNacimiento
                };

                if (body.especialidades != null)
                {
                    List<Especialidad> especialidades = new List<Especialidad>();
                    foreach (var esp in body.especialidades)
                    {
                        Especialidad? especialidad = (await _especialidadRepository.FilterAsync(x => x.Nombre == esp)).FirstOrDefault();
                        
                        if (especialidad != null)
                        {
                            especialidades.Add(especialidad);
                        }
                    }
                    profesionalNuevo.AgregarEspecialidades(especialidades);
                }

                var profesionalCreado = await _profesionalRepository.AddAsync(profesionalNuevo);

                return Ok(profesionalCreado);
            }
        }
        catch (Exception e)
        {
            return BadRequest(e);
        }
    }


    [HttpPost("AsociarProfesionalFacturador")]
    public async Task<IActionResult> AsociarProfesionalFacturador(ProfesionalFacturadorModel body)
    {
        try
        {
            var profesional = (await _profesionalRepository.FilterAsync(x => x.Id == body.idProfesional)).FirstOrDefault();
            var facturador = (await _facturadorRepository.FilterAsync(x => x.Id == body.idFacturador)).FirstOrDefault();

            if (profesional == null || facturador == null)
            {
                return NotFound();
            }
            else
            {
                var profesionalFacturador = new ProfesionalFacturador()
                {
                    Id = Guid.NewGuid(),
                    Profesional = profesional,
                    Facturador = facturador,
                    FechaDesde = DateTime.UtcNow
                };

                var profesionalFacturadorCreado = await _profesionalFacturadorRepository.AddAsync(profesionalFacturador);

                return Ok(profesionalFacturadorCreado);
            }
        }
        catch (Exception e)
        {
            return BadRequest(e);
        }
    }
}