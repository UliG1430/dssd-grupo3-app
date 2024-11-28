namespace Backend.Dto;

public class CargarOrdenForm
{
    public required string Material { get; set; }
    public required int Cantidad { get; set; }
    public required int Zona { get; set; }
    public required int CaseId { get; set; }
    public required int UsuarioId { get; set; }
    public required int PaqueteId { get; set; }
    public required bool Revisado { get; set; }
    public required string Estado { get; set; }
}