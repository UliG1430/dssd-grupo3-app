using System.ComponentModel.DataAnnotations;

namespace api.Model;

public class Validacion
{
    [Key]
    public Guid Id { get; set; }
    public required string Token { get; set; }
    public int NroTransaccion { get; set; }
    public string? NroAfiliado { get; set; }
    public string? NombreApellidoAfiliado { get; set; }
    public string? TipoAfiliatorio { get; set; }
    public DateTime Fecha { get; set; }
    public required string JSONresponse { get; set; }
    public required string Username { get; set; }
}