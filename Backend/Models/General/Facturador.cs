using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;
//using Microsoft.EntityFrameworkCore;
namespace api.Model;

//[Table("Facturadores")]
//[Index(nameof(Nombre), IsUnique = true)]
public class Facturador
{
    [Key]
    public Guid Id {get;set;}
    public required string Nombre {get;set;}
    public required string Direccion {get;set;}
    public required string CuitPrimario {get;set;}
    public required string CuitFacturador {get;set;}
    public required bool EsEstablecimiento {get;set;}
    public Profesional? Profesional {get;set;}
}