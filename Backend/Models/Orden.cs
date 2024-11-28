namespace Backend.Model;

public class Orden
{
    public int Id { get; set; }
    public required string Material { get; set; }
    public required double PesoKg { get; set; }
    public required int PuntoRecoleccionId { get; set; }
    public required DateTime Fecha { get; set; }
    public required int CaseId { get; set; }
    public required int UsuarioId { get; set; }
    public required int paqueteId { get; set; }
    public required bool revisado { get; set; }
    public required string estado { get; set; }

    public PuntoRecoleccion PuntoRecoleccion { get; set; } // Navigation property
    //public string Usuario { get; set; }
}