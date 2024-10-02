namespace Backend.Model;
public class Orden
{
    public required Guid Id { get; set; }
    public required string Material { get; set; }
    public required double PesoKg { get; set; }
    public required string PuntoRecolleccion { get; set; }
    public required DateTime Fecha { get; set; }
    //public string Usuario { get; set; }
}