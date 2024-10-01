using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;

namespace api.Model;

//[Table("Partidos")]
public class Partido
{
    [Key]
    public Guid Id {get;set;}
    public required string Descripcion {get;set;}
    public required string RefIdKlinicos {get;set;}
}