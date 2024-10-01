using System.ComponentModel.DataAnnotations;
namespace api.Model;

public class UsuarioNivelFacturador
{
    [Key]
    public Guid Id {get;set;}
    public required Usuario Usuario {get;set;}
    public required Facturador Facturador {get;set;}
    public required Nivel Nivel {get;set;}
}