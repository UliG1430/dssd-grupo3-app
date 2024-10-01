using System.ComponentModel.DataAnnotations;

namespace api.Model;

public class Paciente
{
    [Key]
    public Guid Id { get; set; }
    public required string TipoDocumento { get; set; }
    public required string NumeroDocumento { get; set; }
    public required string Sexo { get; set; }
    public string? ApellidoNombre { get; set; }
    public string? Mail { get; set; }
    public string? Telefono { get; set; }
    public string? Direccion { get; set; }
}