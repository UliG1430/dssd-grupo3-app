using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;
//using Microsoft.EntityFrameworkCore;
namespace api.Model;

//[Table("Especialidades")]
//[Index(nameof(Codigo), IsUnique = true)]
public class Especialidad
{
    [Key]
    public Guid Id {get;set;}
    public required string Codigo {get;set;}
    public required string Nombre {get;set;}
    public required string TipoEspecialidad {get;set;}
    public required int IdProfesionIOMA {get;set;}
    public required bool Vigente {get;set;}
}