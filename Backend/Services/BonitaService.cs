using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;

namespace Backend.Services{
public class BonitaService
{
    private readonly HttpClient _httpClient;

    public BonitaService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

   public async Task<string> LoginAsync(string username, string password)
   {
       var loginUrl = "http://localhost:8080/bonita/loginservice";  // URL de login de Bonita

       var formData = new FormUrlEncodedContent(new[]
       {
           new KeyValuePair<string, string>("username", username),
           new KeyValuePair<string, string>("password", password),
           new KeyValuePair<string, string>("redirect", "false")  // Evita redirecciones en Bonita
       });

       try
       {
           var response = await _httpClient.PostAsync(loginUrl, formData);

           if (response.IsSuccessStatusCode)
           {
               if (response.Headers.TryGetValues("Set-Cookie", out var cookies))
               {
                   foreach (var cookie in cookies)
                   {
                       if (cookie.Contains("X-Bonita-API-Token"))
                       {
                           var token = cookie.Split(';')[0].Split('=')[1];
                           Console.WriteLine($"Token de Bonita: {token}");  // Verifica que el token no sea vacío
                           return token;  // Devolvemos el token al controlador
                       }
                   }
               }
               return null;  // Si no encontramos el token
           }
           else
           {
               var content = await response.Content.ReadAsStringAsync();
               throw new System.Exception($"No se pudo autenticar en Bonita. Código de estado: {response.StatusCode}. Contenido: {content}");
           }
       }
       catch (System.Exception ex)
       {
           throw new System.Exception($"Error conectando a la API de Bonita: {ex.Message}");
       }
   }
}
}