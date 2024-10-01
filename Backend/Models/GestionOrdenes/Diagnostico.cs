using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;

namespace api.Model;

//[Table("Diagnosticos")]
public class Diagnostico
{
    [Key]
    public Guid Id {get;set;}
    public required string Codigo {get;set;}
    public required string Descripcion {get;set;}
    public required string TipoPatologia {get;set;}
    public required bool Activo {get;set;}
}