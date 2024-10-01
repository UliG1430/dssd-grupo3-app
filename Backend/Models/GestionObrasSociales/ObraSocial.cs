using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;
//using Microsoft.EntityFrameworkCore;

namespace api.Model;

//[Table("ObrasSociales")]
//[Index(nameof(Nombre), IsUnique = true)]
public class ObraSocial
{
    [Key]
    public Guid Id { get; set; }
    public required string Nombre { get; set; }
    public bool Activa { get; set; }
}