using Backend.Dto;
using Backend.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BonitaController : ControllerBase
    {
        private readonly BonitaService _bonitaService;

        public BonitaController(BonitaService bonitaService)
        {
            _bonitaService = bonitaService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] BonitaLoginRequest request)
        {
            try
            {
                var token = await _bonitaService.LoginAsync(request.Username, request.Password);
                if (token != null)
                {
                    // Retornamos el token al frontend
                    return Ok(new { Token = token });
                }
                else
                {
                    return Unauthorized("No se pudo obtener el token de Bonita.");
                }
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }

    // DTO para el login
    public class BonitaLoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
