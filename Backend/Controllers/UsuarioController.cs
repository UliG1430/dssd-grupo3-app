using Backend.Dto;
using Backend.Model;
using Backend.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[ApiController]
[Route("[controller]")]
public class UsuarioController : ControllerBase
{
    private readonly UsuarioRepository _usuarioRepository;

    public UsuarioController(UsuarioRepository usuarioRepository)
    {
        _usuarioRepository = usuarioRepository;
    }

    [HttpGet("{username}")]
    //[Authorize]
    public async Task<IActionResult> getUser(string username)
    {
        try
        {
            var usuario = await _usuarioRepository.GetUsuarioByUsernameAsync(username);

            return Ok(usuario);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    // New endpoint to update user
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser(int id, [FromBody] UpdateUserDto updateUserDto)
    {
        try
        {
            // Find the user by ID
            var usuario = await _usuarioRepository.GetByIdAsync(id);
            if (usuario == null)
            {
                return NotFound(new { message = "Usuario not found" });
            }

            // Update the fields
            if (updateUserDto.Rol.Equals('R'))
            {
                usuario.paqueteId = updateUserDto.PaqueteId;
                usuario.seleccionoPaquete = updateUserDto.SeleccionoPaquete;
            } else {
                usuario.caseId = updateUserDto.CaseId;
                usuario.comenzoRecorrido = updateUserDto.ComenzoRecorrido;
            }

            // Save the changes
            _usuarioRepository.Update(usuario);
            await _usuarioRepository.SaveChangesAsync();

            return Ok(new { message = "Usuario updated successfully", usuario });
        }
        catch (Exception e)
        {
            return BadRequest(new { message = "Error updating Usuario", error = e.Message });
        }
    }
}