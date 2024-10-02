namespace Backend.Dto;

public class CargarOrdenForm
{
    public required string Material { get; set; }
    public required int Cantidad { get; set; }
    public required string Zona { get; set; }
}