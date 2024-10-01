using api.Dto;
using api.Model;
using ApiACEAPP.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[ApiController]
[Route("[controller]")]
public class ObraSocialController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<PacienteController> _logger;
    private readonly ObraSocialRepository _obraSocialRepository;

    public ObraSocialController(IConfiguration configuration, ILogger<PacienteController> logger,
                                ObraSocialRepository obraSocialRepository)
    {
        _configuration = configuration;
        _logger = logger;
        _obraSocialRepository = obraSocialRepository;
    }

    [HttpPost("AgregarObraSocial")]
    public async Task<IActionResult> AgregarObraSocial(ObraSocialModel body)
    {
        try
        {
            var obraSocial = (await _obraSocialRepository.FilterAsync(x => x.Nombre == body.nombre)).FirstOrDefault();

            if (obraSocial != null)
            {
                // el nombre ya esta en uso
                return Ok(false);
            }
            else
            {
                var obraSocialNueva = new ObraSocial()
                {
                    Id = Guid.NewGuid(),
                    Nombre = body.nombre,
                    Activa = true
                };

                var obraSocialCreada = await _obraSocialRepository.AddAsync(obraSocialNueva);

                return Ok(obraSocialCreada);
            }
        }
        catch (Exception e)
        {
            return BadRequest(e);
        }
    }

    [HttpGet("ListarObrasSociales")]
    [Authorize]
    public async Task<IActionResult> ListarObrasSociales()
    {
        try
        {
            var obrasSociales = await _obraSocialRepository.FilterAsync(x => x.Activa);

            return Ok(obrasSociales);
        }
        catch (Exception e)
        {
            return BadRequest(e);
        }
    }
}