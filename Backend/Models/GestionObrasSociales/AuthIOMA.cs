using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;

namespace api.Model;

//[Table("AuthIOMA")]
public class AuthIOMA
{
    [Key]
    public Guid Id {get;set;}
    public DateTime FechaSolicitud {get;set;}
    public required string AccessToken {get;set;}
    public int ExpiresIn {get;set;}
    public required string TokenType {get;set;}
    public required string Scope {get;set;}
}