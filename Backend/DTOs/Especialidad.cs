namespace api.Dto;

public class AgregarEspecialidadModel
{
    public required string Codigo { get; set; }
    public required string Nombre { get; set; }
    public required string TipoEspecialidad { get; set; }
    public required int IdProfesionIOMA { get; set; }
    public required bool Vigente { get; set; }
}