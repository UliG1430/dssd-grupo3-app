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
}