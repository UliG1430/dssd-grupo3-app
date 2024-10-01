using api.Dto;
using api.Model;
using ApiACEAPP.Repositories;
using ApiACEAPP.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[ApiController]
[Route("[controller]")]
public class PacienteController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<PacienteController> _logger;
    private readonly PacienteRepository _pacienteRepository;
    private readonly ValidacionRepository _validacionRepository;
    private readonly AfiliacionRepository _afiliacionRepository;
    private readonly ObraSocialRepository _obraSocialRepository;
    private readonly IOMAService _IOMAService;
    public PacienteController(IConfiguration configuration, ILogger<PacienteController> logger,
                              PacienteRepository pacienteRepository, ValidacionRepository validacionRepository,
                              AfiliacionRepository afiliacionRepository, ObraSocialRepository obraSocialRepository,
                              IOMAService IOMAService)
    {
        _configuration = configuration;
        _logger = logger;
        _pacienteRepository = pacienteRepository;
        _validacionRepository = validacionRepository;
        _afiliacionRepository = afiliacionRepository;
        _obraSocialRepository = obraSocialRepository;
        _IOMAService = IOMAService;
    }

    private async Task<Paciente> GetPaciente(ValidarPaciente validarPaciente, ValidarPacienteResponse response)
    {
        Paciente? paciente;

        //Buscar
        if (validarPaciente.TipoDocumento == "Carnet de Afiliado")
        {
            paciente = await _pacienteRepository.GetPacienteByDocumento(response.tipoDocumentoDescripcion,
                                                                        response.numeroDocumento.ToString(),
                                                                        response.sexo.ToString());
        }
        else
        {
            paciente = await _pacienteRepository.GetPacienteByDocumento(validarPaciente.TipoDocumento,
                                                                        validarPaciente.NumeroDocumento,
                                                                        validarPaciente.Sexo);
        }

        //No encontré, crear
        if (paciente == null)
        {
            paciente = new Paciente() {
                Id = Guid.NewGuid(),
                TipoDocumento = validarPaciente.TipoDocumento == "Carnet de Afiliado" ? response.tipoDocumentoDescripcion : validarPaciente.TipoDocumento,
                NumeroDocumento = validarPaciente.TipoDocumento == "Carnet de Afiliado" ? response.numeroDocumento.ToString() : validarPaciente.NumeroDocumento,
                Sexo = validarPaciente.Sexo,
                ApellidoNombre = null,
                Mail = null,
                Telefono = null,
                Direccion = null
            };
            paciente = await _pacienteRepository.AddAsync(paciente);
        }

        return paciente;
    }

    private async Task<Paciente> ActualizarPaciente(Paciente paciente, ValidarPacienteResponse response, ValidarPaciente validarPaciente)
    {
        if (paciente.ApellidoNombre == null || paciente.ApellidoNombre != response.apellidoNombre)
        {
            paciente.ApellidoNombre = response.apellidoNombre;
            await _pacienteRepository.UpdateAsync(paciente, paciente);
        }

        Afiliacion? afiliacion = (await _afiliacionRepository.FilterAsync(x => x.Paciente == paciente &&
                                                                        x.ObraSocial.Nombre == validarPaciente.ObraSocial)).FirstOrDefault();
        
        if (afiliacion == null)
        {
            ObraSocial? obraSocial = await _obraSocialRepository.GetByName(validarPaciente.ObraSocial);

            afiliacion = new Afiliacion()
            {
                Id = Guid.NewGuid(),
                NroAfiliado = response.numeroAfiliado,
                ObraSocial = obraSocial,
                Paciente = paciente,
                TipoAfiliacion = response.tipoAfiliatorio
            };

            await _afiliacionRepository.AddAsync(afiliacion);
        }
        else
        {
            if (afiliacion.NroAfiliado != response.numeroAfiliado || afiliacion.TipoAfiliacion != response.tipoAfiliatorio)
            {
                afiliacion.NroAfiliado = response.numeroAfiliado;
                afiliacion.TipoAfiliacion = response.tipoAfiliatorio;
                await _afiliacionRepository.UpdateAsync(afiliacion, afiliacion);
            }
        }

        return paciente;
    }

    [HttpPost("ValidarPaciente")]
    [Authorize]
    public async Task<IActionResult> ValidarPaciente(ValidarPaciente validarPaciente)
    {
        ServiceResult<ValidarPacienteResponse> response;

        validarPaciente.Sexo = validarPaciente.Sexo == "Femenino" ? "2" : "1";

        try
        {
            switch (validarPaciente.ObraSocial)
            {
                case "IOMA":
                    response = await _IOMAService.ValidarPaciente(validarPaciente);
                    break;
                case "PAMI":
                    response = await _IOMAService.ValidarPaciente(validarPaciente);
                    break;
                default:
                    return BadRequest("Obra social no soportada");
            }

            if (!response.Success)
            {
                return BadRequest(response.ErrorMessage);
            }

            Paciente paciente = await GetPaciente(validarPaciente, response.Data);

            paciente = await ActualizarPaciente(paciente, response.Data, validarPaciente);

            PacienteResultadoValidacion pacienteResultadoValidacion = new PacienteResultadoValidacion(response.Data.apellidoNombre,
                                                                                                      response.Data.numeroAfiliado,
                                                                                                      response.Data.tipoAfiliatorio,
                                                                                                      response.Data.partidoDescripcion,
                                                                                                      response.Data.localidadDescripcion,
                                                                                                      response.Data.nroTransaccion,
                                                                                                      paciente.Id, paciente.Mail,
                                                                                                      paciente.Telefono, paciente.Direccion);

            return Ok(pacienteResultadoValidacion);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
        finally
        {
            //_logger.LogInformation("ValidarPaciente");
        }
    }

    [HttpPost("CargarDatosAdicionales")]
    [Authorize]
    public async Task<IActionResult> CargarDatosAdicionales(PacienteDatosAdicionales body)
    {
        try
        {
            Paciente? paciente = await _pacienteRepository.GetAsync(body.PacienteId);
            if (paciente == null)
            {
                return NotFound("Paciente no encontrado");
            }
            paciente.Mail = body.Mail;
            paciente.Telefono = body.Telefono;
            paciente.Direccion = body.Direccion;
            await _pacienteRepository.UpdateAsync(paciente, paciente);
            return Ok(true);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
        finally
        {
            //_logger.LogInformation("CargarDatosAdicionales");
        }
    }
}