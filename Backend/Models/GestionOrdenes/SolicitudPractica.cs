using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;

namespace api.Model;

//[Table("SolicitudPracticas")]
public class SolicitudPractica
{
    [Key]
    public Guid Id { get; set; }
    public Guid SolicitudId { get; set; }
    public required Practica Practica { get; set; }
    public Diagnostico? Diagnostico { get; set; }
    public int Cantidad { get; set; }
}