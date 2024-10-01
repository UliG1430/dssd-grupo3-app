using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace api.Helpers.TokenReaderHelper;

public static class TokenReaderHelper
{
    public static string readToken(string token)
    {
        try
        {
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token);
            var tokenS = jsonToken as JwtSecurityToken;
            var username = tokenS?.Claims.First(claim => claim.Type == ClaimTypes.NameIdentifier).Value;
            if (username != null && tokenS?.ValidTo > DateTime.Now)
            {
                return username;
            }
            else
            {
                return "0"; // retorno 0 si el contenido del token no corresponde con un correo o si está vencido
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return "0"; // retorno 0 si el token no es correcto
        }
    }
}