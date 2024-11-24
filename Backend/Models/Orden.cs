namespace Backend.Model;

public class Orden
{
    public int Id { get; set; }
    public required string Material { get; set; }
    public required double PesoKg { get; set; }
    public required string PuntoRecoleccion { get; set; }
    public required DateTime Fecha { get; set; }
    public required int CaseId { get; set; }
    public required int UsuarioId { get; set; }
    //public string Usuario { get; set; }
}