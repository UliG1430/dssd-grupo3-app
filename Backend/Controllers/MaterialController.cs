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

    [HttpGet("materiales")]
    //[Authorize]
    public async Task<IActionResult> getMateriales()
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

    [HttpPut("Stock/Add/{id}")]
    public async Task<IActionResult> AddToStockMaterial(int id, [FromBody] UpdateMaterialStockDto body)
    {
         try
         {
             // Retrieve the material
             var material = await _materialRepository.GetByIdAsync(id);
             if (material == null)
             {
                 return NotFound(new { message = $"Material with Id {id} not found." });
             }

             // Add to the current stock
             material.StockActual += body.Cantidad;

             // Save changes
             _materialRepository.Update(material);
             await _materialRepository.SaveChangesAsync();

             return Ok(new { message = "Material stock increased successfully", material });
         }
         catch (Exception e)
         {
             return BadRequest(new { message = "Error increasing material stock.", error = e.Message });
         }
    }

    [HttpPut("Stock/Reduce/{id}")]
    public async Task<IActionResult> ReduceStockMaterial(int id, [FromBody] UpdateMaterialStockDto body)
    {
     try
     {
         // Retrieve the material
         var material = await _materialRepository.GetByIdAsync(id);
         if (material == null)
         {
             return NotFound(new { message = $"Material with Id {id} not found." });
         }

         // Ensure stock does not go negative
         if (material.StockActual < body.Cantidad)
         {
             return BadRequest(new { message = "Not enough stock to complete the reduction." });
         }

         // Reduce the current stock
         material.StockActual -= body.Cantidad;

         // Save changes
         _materialRepository.Update(material);
         await _materialRepository.SaveChangesAsync();

         return Ok(new { message = "Material stock reduced successfully", material });
     }
     catch (Exception e)
     {
         return BadRequest(new { message = "Error reducing material stock.", error = e.Message });
     }
    }

}