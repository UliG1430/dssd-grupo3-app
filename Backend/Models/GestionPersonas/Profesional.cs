using System.ComponentModel.DataAnnotations;

namespace api.Model;

public class Profesional
{
    [Key]
    public Guid Id {get;set;}
    public required string Cuit {get;set;}
    public required string MatriculaProvincial {get;set;}
    public required string MatriculaNacional {get;set;}
    public required string Apellidos {get;set;}
    public required string Nombres {get;set;}
    public required List<ProfesionalEspecialidad> Especialidades {get;set;}
    public string? Domicilio {get;set;}
    public string? Telefono {get;set;}
    public string? Email {get;set;}
    public DateTime? FechaNacimiento {get;set;}

    public void AgregarEspecialidades(List<Especialidad> especialidades)
    {
        foreach (var esp in especialidades)
        {
            ProfesionalEspecialidad solicitudPractica = new ProfesionalEspecialidad()
            {
                Id = Guid.NewGuid(),
                Profesional = this,
                Especialidad = esp,
                Vigente = true
            };
            this.Especialidades.Add(solicitudPractica);
        }
    }

}