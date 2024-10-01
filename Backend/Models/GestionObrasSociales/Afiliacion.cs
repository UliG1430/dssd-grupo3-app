using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;
//using Microsoft.EntityFrameworkCore;

namespace api.Model;

//[Table("Afiliaciones")]
//[Index(nameof(Paciente), nameof(ObraSocial), nameof(NroAfiliado))]
public class Afiliacion
{
    [Key]
    public Guid Id { get; set; }
    public required Paciente Paciente { get; set; }
    public required ObraSocial ObraSocial { get; set; }
    public required string NroAfiliado { get; set; }
    public required string TipoAfiliacion { get; set; }
}