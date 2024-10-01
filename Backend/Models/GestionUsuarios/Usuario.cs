using System.ComponentModel.DataAnnotations;

namespace api.Model;
public class Usuario
{
    [Key]
    public Guid Id {get;set;}
    //Unico
    public required string Username {get;set;}
    public required string Password {get;set;}
    public required string Email {get;set;}
}