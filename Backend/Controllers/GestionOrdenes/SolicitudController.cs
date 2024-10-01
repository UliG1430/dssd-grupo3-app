using api.Dto;
using api.Model;
using ApiACEAPP.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[ApiController]
[Route("[controller]")]
public class SolicitudController : ControllerBase
{
    private readonly ILogger<SolicitudController> _logger;
    private readonly SolicitudRepository _solicitudRepository;
    private readonly ValidacionRepository _validacionRepository;
    private readonly FacturadorRepository _facturadorRepository;
    private readonly ProfesionalRepository _profesionalRepository;
    private readonly DiagnosticoRepository _diagnosticoRepository;
    private readonly PracticaRepository _practicaRepository;

    public SolicitudController(ILogger<SolicitudController> logger,
                               SolicitudRepository solicitudRepository,
                               ValidacionRepository validacionRepository,
                               FacturadorRepository facturadorRepository,
                               ProfesionalRepository profesionalRepository,
                               DiagnosticoRepository diagnosticoRepository,
                               PracticaRepository practicaRepository)
    {
        _logger = logger;
        _solicitudRepository = solicitudRepository;
        _validacionRepository = validacionRepository;
        _facturadorRepository = facturadorRepository;
        _profesionalRepository = profesionalRepository;
        _diagnosticoRepository = diagnosticoRepository;
        _practicaRepository = practicaRepository;
    }

    private async Task<IActionResult> AgregarPrestaciones(Solicitud solicitud, List<PracticaASolicitar> prestaciones)
    {
        foreach (var prestacion in prestaciones)
        {
            Practica? practica = (await _practicaRepository.FilterAsync(x => x.Codigo == prestacion.Codigo)).FirstOrDefault();

            if (practica == null)
            {
                return BadRequest("Prestación " + prestacion.Codigo + "no encontrada");
            }

            Diagnostico? diagnosticoP = (await _diagnosticoRepository.FilterAsync(x => x.Id == prestacion.IdDiagnostico)).FirstOrDefault();

            if (diagnosticoP == null)
            {
                return BadRequest("Diagnóstico no encontrado");
            }

            SolicitudPractica solicitudPractica = new SolicitudPractica()
            {
                Id = Guid.NewGuid(),
                SolicitudId = solicitud.Id,
                Practica = practica,
                Diagnostico = diagnosticoP,
                Cantidad = prestacion.Cantidad
            };
            solicitud.Prestaciones.Add(solicitudPractica);
        }
        return Ok(true);
    }

    [HttpPost("GetSolicitudesFacturador")]
    [Authorize]
    public async Task<IActionResult> GetSolicitudesFacturador (GetSolicitudesFacturadorModel body)
    {
        try
        {
            var solicitudes = await _solicitudRepository.FilterAsync(x => x.Facturador.Id == body.idFacturador, includes: "Prestaciones,Prestaciones.Practica,Prestaciones.Diagnostico,Validacion,Facturador,Efector,Diagnostico");
            List<SolicitudesResponse> solicitudesResponse = new List<SolicitudesResponse>();
            foreach (var solicitud in solicitudes)
            {
                List<SolicitudPracticaResponse> prestacionesResponse = new List<SolicitudPracticaResponse>();
                foreach (var prestacion in solicitud.Prestaciones)
                {
                    prestacionesResponse.Add(new SolicitudPracticaResponse()
                    {
                        Id = prestacion.Id,
                        SolicitudId = prestacion.SolicitudId,
                        CodigoPractica = prestacion.Practica.Codigo,
                        DescripcionPractica = prestacion.Practica.Descripcion,
                        DescripcionDiagnostico = prestacion.Diagnostico?.Descripcion,
                        Cantidad = prestacion.Cantidad
                    });
                }
                solicitudesResponse.Add(new SolicitudesResponse()
                {
                    Id = solicitud.Id,
                    MatriculaEfector = solicitud.Efector.MatriculaProvincial,
                    NombreApellidoEfector = solicitud.Efector.Nombres + " " + solicitud.Efector.Apellidos,
                    MatriculaPrescriptor = solicitud.MatriculaPrescriptor,
                    NombreApellidoPrescriptor = solicitud.NombreApellidoPrescriptor,
                    Validacion = solicitud.Validacion,
                    Username = solicitud.Username,
                    Facturador = solicitud.Facturador,
                    TipoIngreso = solicitud.Ingreso.ToString(),
                    Fecha = solicitud.Fecha,
                    Diagnostico = solicitud.Diagnostico?.Descripcion,
                    Prestaciones = prestacionesResponse
                });
            }
            return Ok(solicitudesResponse);
        }
        catch (Exception ex)
        {
            //_logger.LogError(ex, "Error al obtener las solicitudes del facturador");
            return StatusCode(500, "Error al obtener las solicitudes del facturador");
        }
    }

    [HttpGet("GetTiposIngreso")]
    [Authorize] 
    public async Task<IActionResult> GetTiposIngreso()
    {
        try
        {
            //Crear un diccionario que retorne, para cada tipo de ingreso, su descripción y valor
            var tiposIngreso = Enum.GetValues(typeof(TiposIngreso))
                .Cast<TiposIngreso>()
                .Select(x => new { Descripcion = x.ToString(), Valor = (int)x })
                .ToList();
            return Ok(tiposIngreso);
        }
        catch (Exception ex)
        {
            //_logger.LogError(ex, "Error al obtener los tipos de ingreso");
            return StatusCode(500, "Error al obtener los tipos de ingreso");
        }
    }

    [HttpGet("GetDiagnosticos")]
    [Authorize]
    public async Task<IActionResult> GetDiagnosticos()
    {
        try
        {
            var diagnosticos = await _diagnosticoRepository.FilterAsync(x => x.Activo);
            return Ok(diagnosticos);
        }
        catch (Exception ex)
        {
            //_logger.LogError(ex, "Error al obtener los diagnósticos");
            return StatusCode(500, "Error al obtener los diagnósticos");
        }
    }

    [HttpPost("AgregarDiagnostico")]
    public async Task<IActionResult> AgregarDiagnostico(DiagnosticosModel body)
    {
        try
        {
            var diagnosticoNuevo = new Diagnostico()
            {
                Id = Guid.NewGuid(),
                Codigo = body.Codigo,
                Descripcion = body.Descripcion,
                TipoPatologia = body.Patologia,
                Activo = true
            };

            var diagnosticoCreado = await _diagnosticoRepository.AddAsync(diagnosticoNuevo);

            return Ok(true);
        }
        catch (Exception ex)
        {
            //_logger.LogError(ex, "Error al agregar el diagnóstico");
            return StatusCode(500, "Error al agregar el diagnóstico");
        }
    }

    [HttpPost("AgregarSolicitud")]
    [Authorize]
    public async Task<IActionResult> AgregarSolicitud(SolicitudModel body)
    {
        try
        {
            Validacion? validacion = (await _validacionRepository.FilterAsync(x => x.NroTransaccion == body.ValidacionId)).FirstOrDefault();
            if (validacion == null)
            {
                return BadRequest("Validacion no encontrada");
            }

            Facturador? facturador = (await _facturadorRepository.FilterAsync(x => x.Id == body.IdFacturador)).FirstOrDefault();
            if (facturador == null)
            {
                return BadRequest("Facturador no encontrado");
            }

            Profesional? efector = (await _profesionalRepository.FilterAsync(x => x.MatriculaProvincial == body.MatriculaEfector)).FirstOrDefault();
            if (efector == null)
            {
                return BadRequest("Efector no encontrado");
            }

            Diagnostico? diagnostico = null;
            if (body.IdDiagnostico != null)
            {
                diagnostico = (await _diagnosticoRepository.FilterAsync(x => x.Id == body.IdDiagnostico)).FirstOrDefault();
                if (diagnostico == null)
                {
                    return BadRequest("Diagnóstico no encontrado");
                }
            }

            var solicitudNueva = new Solicitud()
            {
                Id = Guid.NewGuid(),
                Efector = efector,
                MatriculaPrescriptor = body.MatriculaPrescriptor,
                NombreApellidoPrescriptor = body.NombreApellidoPrescriptor,
                Validacion = validacion,
                Username = body.Username,
                Facturador = facturador,
                Ingreso = (TiposIngreso)body.TipoIngreso,
                Diagnostico = diagnostico,
                Prestaciones = new List<SolicitudPractica>(),
                Fecha = DateTime.UtcNow
            };

            await AgregarPrestaciones(solicitudNueva, body.Prestaciones);

            var solicitudCreada = await _solicitudRepository.AddAsync(solicitudNueva);

            return Ok(true);
        }
        catch (Exception ex)
        {
            //_logger.LogError(ex, "Error al agregar la solicitud");
            return StatusCode(500, "Error al agregar la solicitud");
        }
    }
}