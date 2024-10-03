using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;

namespace Backend.Services
{
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

                // Verificamos si la solicitud fue exitosa
                if (response.IsSuccessStatusCode)
                {
                    // Intentamos obtener las cookies
                    if (response.Headers.TryGetValues("Set-Cookie", out var cookies))
                    {
                        Console.WriteLine($"Cookies recibidas: {string.Join(", ", cookies)}");  // Verificamos todas las cookies recibidas

                        // Buscamos el token válido
                        foreach (var cookie in cookies)
                        {
                            // Verificamos si encontramos el token en las cookies
                            if (cookie.Contains("X-Bonita-API-Token"))
                            {
                                var token = cookie.Split(';')[0].Split('=')[1];

                                // Verificamos que el token no esté vacío
                                if (!string.IsNullOrEmpty(token))
                                {
                                    Console.WriteLine($"Token de Bonita: {token}");  // Verificamos que el token no sea vacío
                                    return token;  // Devolvemos el token al controlador
                                }
                            }
                        }
                        Console.WriteLine("No se encontró un token válido en las cookies.");
                    }
                    else
                    {
                        Console.WriteLine("No se encontraron cookies en la respuesta.");
                    }

                    // Si no encontramos el token válido en las cookies, devolvemos null
                    return null;
                }
                else
                {
                    // Si la respuesta no fue exitosa, capturamos el contenido del error
                    var content = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Error en la autenticación: {content}");
                    throw new System.Exception($"No se pudo autenticar en Bonita. Código de estado: {response.StatusCode}. Contenido: {content}");
                }
            }
            catch (System.Exception ex)
            {
                Console.WriteLine($"Error conectando a la API de Bonita: {ex.Message}");
                throw new System.Exception($"Error conectando a la API de Bonita: {ex.Message}");
            }
        }
    }
}
