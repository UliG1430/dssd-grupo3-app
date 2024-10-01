using System.ComponentModel.DataAnnotations;
namespace api.Model;

public class Nivel
{
    [Key]
    public Guid Id {get;set;}
    public required string Nombre {get;set;}
}