using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;

namespace api.Model;

//[Table("Localidades")]
public class Localidad
{
    [Key]
    public Guid Id {get;set;}
    public required string Nombre {get;set;}
    public required string CodigoPostal {get;set;}
    public required Partido Partido {get;set;}
}