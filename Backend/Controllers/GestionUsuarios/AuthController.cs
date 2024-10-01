using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using api.Dto;
using api.Helpers.TokenReaderHelper;
using api.Model;
using ApiACEAPP.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace ApiACEAPP.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<AuthController> _logger;
    private readonly UsuarioRepository _usuarioRepository;
    private readonly UsuarioNivelFacturadorRepository _usuarioNivelFacturadorRepository;
    public AuthController(IConfiguration configuration, ILogger<AuthController> logger, UsuarioRepository usuarioRepository, UsuarioNivelFacturadorRepository usuarioNivelFacturadorRepository)
    {
        _configuration = configuration;
        _logger = logger;
        _usuarioRepository = usuarioRepository;
        _usuarioNivelFacturadorRepository = usuarioNivelFacturadorRepository;
    }

    private string GenerarTokenAuth(string username)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["ConfigurationJwt:SecretKey"]!));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier,username)
        };
        DateTime expirationTime = DateTime.Now.AddMinutes(90); //tiempo de expiración del TOKEN = 90 minutos
        var secToken = new JwtSecurityToken(
            _configuration["ConfigurationJwt:Issuer"],
            null,
            claims,
            expires: expirationTime,
            signingCredentials: credentials
        );
        var token = new JwtSecurityTokenHandler().WriteToken(secToken);
        return token;
    }

    private static string HashPassword(string password)
    {
        byte[] data = Encoding.UTF8.GetBytes(password); // Cambié a UTF8 para evitar problemas con caracteres fuera de ASCII
        data = System.Security.Cryptography.SHA256.HashData(data);
        string hash = Convert.ToBase64String(data); // Convertimos el hash a Base64
        return hash;
    }

    [HttpPost("Register")]
    public async Task<IActionResult> Register(RegisterModel body)
    {
        try
        {
            var user = (await _usuarioRepository.FilterAsync(x => x.Email == body.username)).FirstOrDefault();

            if (user != null)
            {
                // el username ya esta en uso
                return Ok(false);
            }
            else
            {
                var userNuevo = new Usuario()
                {
                    Id = Guid.NewGuid(),
                    Email = body.email,
                    Username = body.username,
                    Password = HashPassword(body.username),
                };

                var userCreado = await _usuarioRepository.AddAsync(userNuevo);

                return Ok(true);
            }
        }
        catch (Exception e)
        {
            return BadRequest(e);
        }
    }

    [HttpPost("Login")]
    public async Task<IActionResult> Login(LoginModel body)
    {
        try
        {
            //Buscar usuario por username
            var user = (await _usuarioRepository.FilterAsync(x => x.Username == body.username)).FirstOrDefault();

            if (user == null)
            {
                return NotFound();
            }
            else
            {
                if (body.password == user.Username && HashPassword(body.password) == user.Password)
                {
                    var token = GenerarTokenAuth(user.Username);
                    var response = new ResponseLoginModel(){
                        token = token,
                        firstLogin = true
                    };
                    return Ok(response);
                }
                if (HashPassword(body.password) == user.Password)
                {
                    var token = GenerarTokenAuth(user.Username);
                    var response = new ResponseLoginModel(){
                        token = token,
                        firstLogin = false
                    };
                    return Ok(response);
                }
                else
                {
                    return NotFound();
                }
            }
        }
        catch (Exception e)
        {
            return BadRequest(e);
        }
    }

    [HttpPost("ResetPassword")]
    public async Task<IActionResult> ResetPassword(ResetPasswordModel body)
    {
        try
        {
            var user = (await _usuarioRepository.FilterAsync(x => x.Username == body.username)).FirstOrDefault();

            if (user == null)
            {
                return NotFound("invalid");
            }
            else
            {
                user.Password = HashPassword(user.Username);
                await _usuarioRepository.UpdateAsync(user,user);
                return Ok("success");
            }
        }
        catch (Exception e)
        {
            return BadRequest(e);
        }
    }

    [HttpPost("ChangePassword"), Authorize]
    public async Task<IActionResult> ChangePassword(ChangePasswordModel body)
    {
        try
        {
            string authHeader = HttpContext.Request.Headers["Authorization"].ToString();
            if (authHeader.IsNullOrEmpty() || !authHeader.StartsWith("Bearer "))
            {
                return Unauthorized("invalid");
            }

            string token = authHeader.Substring("Bearer ".Length).Trim();
            string payload = TokenReaderHelper.readToken(token);

            if (payload == "0")
            {
                return Unauthorized("invalid");
            }

            var user = (await _usuarioRepository.FilterAsync(x => x.Username == payload)).FirstOrDefault();

            if (user == null)
            {
                return NotFound("invalid");
            }
            else
            {
                if (user.Username == body.password)
                {
                    return Ok("same");
                }
                else
                {
                    user.Password = HashPassword(body.password);
                    await _usuarioRepository.UpdateAsync(user,user);
                    return Ok("success");
                }
            }
        }
        catch (Exception e)
        {
            return BadRequest(e);
        }
    }

    [HttpGet("GetUsuarioNivelFacturador"), Authorize]
    public async Task<IActionResult> GetUsuarioNivelFacturador()
    {
        try
        {
            var authHeader = HttpContext.Request.Headers["Authorization"].ToString();
            if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
            {
                return Unauthorized();
            }

            var token = authHeader.Substring("Bearer ".Length).Trim();

            // Leer el payload del token
            var payload = TokenReaderHelper.readToken(token);

            if (payload == "0")
            {
                return Unauthorized();
            }

            var user = (await _usuarioRepository.FilterAsync(x => x.Username == payload)).FirstOrDefault();

            if (user == null)
            {
                return NotFound();
            }
            else
            {
                var usuNivEst = await _usuarioNivelFacturadorRepository.FilterAsync(
                                                    filtro: x => x.Usuario.Id == user.Id,
                                                    includes: "Nivel,Facturador,Facturador.Profesional");
                return Ok(usuNivEst);
            }
        }
        catch (Exception e)
        {
            return BadRequest(e);
        }
    }
}