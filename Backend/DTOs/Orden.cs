namespace Backend.Dto;

public class CargarOrdenForm
{
    public required string Material { get; set; }
    public required int Cantidad { get; set; }
    public required string Zona { get; set; }
    public required int CaseId { get; set; }
    public required int UsuarioId { get; set; }
}