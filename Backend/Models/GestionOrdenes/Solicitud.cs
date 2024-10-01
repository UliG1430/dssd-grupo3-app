using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;
using api.Dto;

namespace api.Model;

public enum TiposIngreso {
    Guardia,
    Consultorios_Externos,
    Diagnostico_Por_Imagenes,
    Prequirurgico,
    Anatomia_Patologica
}

//[Table("Solicitudes")]
public class Solicitud
{
    [Key]
    public Guid Id {get;set;}
    public required Profesional Efector {get;set;}
    public string? MatriculaPrescriptor {get;set;}
    public string? NombreApellidoPrescriptor {get;set;}
    public required Validacion Validacion {get;set;}
    public required string Username {get;set;}
    public required Facturador Facturador {get;set;}
    [EnumDataType(typeof(TiposIngreso))]
    public TiposIngreso Ingreso {get;set;}
    public DateTime Fecha {get;set;}
    public Diagnostico? Diagnostico {get;set;}
    public required List<SolicitudPractica> Prestaciones {get;set;}
}