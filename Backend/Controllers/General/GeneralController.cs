using Microsoft.AspNetCore.Mvc;
using api.Model;
using api.Dto;
using ApiACEAPP.Repositories;

namespace api.Controllers;

[ApiController]
[Route("[controller]")]
public class GeneralController : ControllerBase
{
    private readonly IConfiguration _config;
    private readonly FacturadorRepository _facturadorRepository;
    private readonly NivelRepository _nivelRepository;
    private readonly UsuarioRepository _usuarioRepository;
    private readonly UsuarioNivelFacturadorRepository _usuarioNivelFacturadorRepository;
    private readonly ProfesionalRepository _profesionalRepository;

    public GeneralController(IConfiguration config,
                             FacturadorRepository facturadorRepository,
                             NivelRepository nivelRepository,
                             UsuarioRepository usuarioRepository,
                             UsuarioNivelFacturadorRepository usuarioNivelFacturadorRepository,
                             ProfesionalRepository profesionalRepository)
    {
        _config = config;
        _facturadorRepository = facturadorRepository;
        _nivelRepository = nivelRepository;
        _usuarioRepository = usuarioRepository;
        _usuarioNivelFacturadorRepository = usuarioNivelFacturadorRepository;
        _profesionalRepository = profesionalRepository;
    }

    [HttpPost("AgregarNivel")]
    public async Task<IActionResult> AgregarNivel(NivelModel body)
    {
        try
        {
            var nivel = (await _nivelRepository.FilterAsync(x => x.Nombre == body.nombre)).FirstOrDefault();

            if (nivel != null)
            {
                // el nombre ya esta en uso
                return Ok(false);
            }
            else
            {
                var nivelNuevo = new Nivel()
                {
                    Id = Guid.NewGuid(),
                    Nombre = body.nombre,
                };

                var nivelCreado = await _nivelRepository.AddAsync(nivelNuevo);

                return Ok(nivelCreado);
            }
        }
        catch (Exception e)
        {
            return BadRequest(e);
        }
    }

    [HttpPost("AgregarUsuarioNivelFacturador")]
    public async Task<IActionResult> AgregarUsuarioNivelFacturador(UsuarioNivelFacturadorModel body)
    {
        try
        {
            var usuario = await _usuarioRepository.GetAsync(body.idUsuario);
            var nivel = await _nivelRepository.GetAsync(body.idNivel);
            var facturador = await _facturadorRepository.GetAsync(body.idFacturador);

            if (usuario == null || nivel == null || facturador == null)
            {
                return NotFound();
            }
            else
            {
                var usuarioNivelFacturadorNuevo = new UsuarioNivelFacturador()
                {
                    Id = Guid.NewGuid(),
                    Usuario = usuario,
                    Nivel = nivel,
                    Facturador = facturador,
                };

                var usuarioNivelFacturadorCreado = await _usuarioNivelFacturadorRepository.AddAsync(usuarioNivelFacturadorNuevo);

                return Ok(usuarioNivelFacturadorCreado);
            }
        }
        catch (Exception e)
        {
            return BadRequest(e);
        }
    }
}