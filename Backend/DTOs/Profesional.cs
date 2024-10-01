namespace api.Dto;

public class ProfesionalModel
{
    public required string cuit { get; set; }
    public required string matriculaProvincial { get; set; }
    public required string matriculaNacional { get; set; }
    public required string apellidos { get; set; }
    public required string nombres { get; set; }
    public List<string>? especialidades { get; set; }
    public string? domicilio { get; set; }
    public string? telefono { get; set; }
    public string? email { get; set; }
    public DateTime? fechaNacimiento { get; set; }
}