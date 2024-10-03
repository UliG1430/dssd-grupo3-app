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

        private IActionResult CheckToken(string token) {
            if (string.IsNullOrEmpty(token)) {
                return Unauthorized(new { message = "Unauthorized: X-Bonita-API-Token is required" });
            }
            return null;
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

        [HttpGet("process/{processName}")]
        public async Task<IActionResult> getProcessId(string processName, [FromHeader(Name = "X-Bonita-API-Token")] string token)
        {
            Console.WriteLine($"{processName}");
            var tokenCheck = CheckToken(token);
            if (tokenCheck != null) {
                return tokenCheck;
            }
            try {
                _bonitaService.SetToken(token);
                var processId = await _bonitaService.GetProcessIdAsync(processName);
                if (processId != null) {
                    return Ok(new { processId });
                } else {
                    return NotFound("Process not found");
                }
            } catch (System.Exception ex) {
                return StatusCode(500, ex.Message);
            }

        }

        [HttpGet("startprocess/{processId}")]
        public async Task<IActionResult> startProcessById(string processId, [FromHeader(Name = "X-Bonita-API-Token")] string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized(new { message = "Missing X-Bonita-AP-Token" });
            }

            _bonitaService.SetToken(token);

            try {
                var processInstance = await _bonitaService.StartProcessAsync(processId);

                return Ok(new { processInstance });
            } catch (System.Exception ex) {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPost("completeActivity/{caseId}")]
        public async Task<IActionResult> CompleteTask(string caseId, [FromHeader(Name = "X-Bonita-API-Token")] string token)
        {
            try
            {
                _bonitaService.SetToken(token);

                var resultado = await _bonitaService.CompletarActividadAsync(caseId);

                return Ok(new { message = resultado });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpGet("getNextTask/{caseId}")]
        public async Task<IActionResult> nextTaskByCaseId(string caseId, [FromHeader(Name = "X-Bonita-API-Token")] string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized(new { message = "Missing X-Bonita-AP-Token" });
            }

            _bonitaService.SetToken(token);

            try {
                var nextTaskId = await _bonitaService.GetNextTaskAsync(caseId);

                return Ok(new { nextTaskId });
            } catch (System.Exception ex) {
                return StatusCode(500, new { message = ex.Message });
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
