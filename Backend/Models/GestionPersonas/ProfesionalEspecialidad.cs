using System.ComponentModel.DataAnnotations;

namespace api.Model;

public class ProfesionalEspecialidad
{
    [Key]
    public Guid Id {get;set;}
    public required Profesional Profesional {get;set;}
    public required Especialidad Especialidad {get;set;}
    public required bool Vigente {get;set;}
}