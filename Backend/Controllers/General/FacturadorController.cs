using Microsoft.AspNetCore.Mvc;
using api.Model;
using api.Dto;
using ApiACEAPP.Repositories;
using Microsoft.AspNetCore.Authorization;

namespace api.Controllers;

[ApiController]
[Route("[controller]")]
public class FacturadorController : ControllerBase
{
    private readonly IConfiguration _config;
    private readonly FacturadorRepository _facturadorRepository;
    private readonly ProfesionalRepository _profesionalRepository;
    private readonly ProfesionalFacturadorRepository _profesionalFacturadorRepository;

    public FacturadorController(IConfiguration config,
                             FacturadorRepository facturadorRepository,
                             ProfesionalRepository profesionalRepository,
                             ProfesionalFacturadorRepository profesionalFacturadorRepository)
    {
        _config = config;
        _facturadorRepository = facturadorRepository;
        _profesionalRepository = profesionalRepository;
        _profesionalFacturadorRepository = profesionalFacturadorRepository;
    }

    [HttpPost("AgregarFacturador")]
    public async Task<IActionResult> AgregarFacturador(FacturadorModel body)
    {
        try
        {
            var facturador = (await _facturadorRepository.FilterAsync(x => x.Nombre == body.nombre)).FirstOrDefault();

            if (facturador != null)
            {
                // el nombre ya esta en uso
                return Ok(false);
            }
            else
            {
                var facturadorNuevo = new Facturador()
                {
                    Id = Guid.NewGuid(),
                    Nombre = body.nombre,
                    Direccion = body.direccion,
                    CuitPrimario = body.cuitPrimario,
                    CuitFacturador = body.cuitFacturador,
                    EsEstablecimiento = body.esEstablecimiento,
                };

                var facturadorCreado = await _facturadorRepository.AddAsync(facturadorNuevo);

                return Ok(facturadorCreado);
            }
        }
        catch (Exception e)
        {
            return BadRequest(e);
        }
    }

    [HttpPost("AsignarProfesionalCargoFacturador")]
    public async Task<IActionResult> AsignarProfesionalFacturador(ProfesionalFacturadorModel body)
    {
        try
        {
            var profesional = await _profesionalRepository.GetAsync(body.idProfesional);
            var facturador = await _facturadorRepository.GetAsync(body.idFacturador);

            if (profesional == null || facturador == null)
            {
                return NotFound();
            }
            else
            {
                facturador.Profesional = profesional;

                var facturadorActualizado = await _facturadorRepository.UpdateAsync(facturador, facturador);

                return Ok(facturadorActualizado);
            }
        }
        catch (Exception e)
        {
            return BadRequest(e);
        }
    }

    [HttpPost("GetProfesionalesFacturador")]
    [Authorize]
    public async Task<IActionResult> GetProfesionalesFacturador(GetProfesionalesFacturadorBody body)
    {
        try
        {
            var facturador = await _facturadorRepository.GetAsync(body.idFacturador);

            if (facturador == null)
            {
                return NotFound();
            }
            else
            {
                List<ProfesionalFacturador> profesionalesFacturador = (await _profesionalFacturadorRepository.FilterAsync(x => x.Facturador.Id == body.idFacturador, includes: "Profesional")).ToList();
                List<Profesional> profesionales = profesionalesFacturador.Select(x => x.Profesional).ToList();
                return Ok(profesionales);
            }
        }
        catch (Exception e)
        {
            return BadRequest(e);
        }
    }
}