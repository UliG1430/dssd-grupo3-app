using Backend.Dto;
using Backend.Model;
using Backend.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[ApiController]
[Route("[controller]")]
public class MaterialController : ControllerBase
{
    private readonly MaterialRepository _materialRepository;

    public MaterialController(MaterialRepository materialRepository)
    {
        _materialRepository = materialRepository;
    }

    // Endpoint para obtener todos los materiales
    [HttpGet("materiales")]
    //[Authorize]
    public async Task<IActionResult> GetMateriales()
    {
        try
        {
            var materiales = await _materialRepository.GetAllAsync();

            return Ok(materiales);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    // Endpoint para obtener el stock de un material por su código
  [HttpGet("{codMaterial}/stock")]
  public async Task<IActionResult> GetStock(string codMaterial)
  {
      try
      {
          var stock = await _materialRepository.GetStockByCodeAsync(codMaterial);

          if (stock == null)
          {
              return NotFound(new
              {
                  Message = $"El material con código {codMaterial} no fue encontrado."
              });
          }

          return Ok(new
          {
              CodMaterial = codMaterial,
              StockActual = stock // Retorna el stock como double
          });
      }
      catch (Exception e)
      {
          return StatusCode(500, new
          {
              Message = "Ocurrió un error al obtener el stock del material.",
              Error = e.Message
          });
      }
  }
}