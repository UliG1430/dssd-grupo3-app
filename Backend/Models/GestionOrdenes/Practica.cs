using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;

namespace api.Model;

//[Table("Practicas")]
public class Practica
{
    [Key]
    public Guid Id {get;set;}
    public required string Codigo {get;set;}
    public required string Descripcion {get;set;}
    public required string Tipo {get;set;}
    public bool Activa {get;set;}
    public float Monto {get;set;}
}