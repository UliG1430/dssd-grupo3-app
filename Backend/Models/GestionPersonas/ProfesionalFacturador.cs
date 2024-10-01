using System.ComponentModel.DataAnnotations;

namespace api.Model;

public class ProfesionalFacturador
{
    [Key]
    public required Guid Id {get;set;}
    public required Profesional Profesional {get;set;}
    public required Facturador Facturador {get;set;}
    public required DateTime FechaDesde {get;set;}
    public DateTime? FechaHasta {get;set;}
}