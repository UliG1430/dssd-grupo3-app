using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Backend.Services;
public class BonitaService
{
    private readonly HttpClient _httpClient;

    public BonitaService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<string> LoginAsync(string username, string password)
    {
        var loginUrl = "http://localhost:8080/bonita/loginservice";
        var content = new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string, string>("username", username),
            new KeyValuePair<string, string>("password", password),
            new KeyValuePair<string, string>("redirect", "false")
        });

        var response = await _httpClient.PostAsync(loginUrl, content);
        if (response.IsSuccessStatusCode)
        {
            var token = response.Headers.GetValues("X-Bonita-API-Token").FirstOrDefault();
            return token;
        }

        throw new Exception("Error de autenticación en Bonita.");
    }

    public async Task StartProcessAsync(string token, string processId)
    {
        var startUrl = $"http://localhost:8080/bonita/API/bpm/process/{processId}/instantiation";
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _httpClient.PostAsync(startUrl, null);
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception("Error al iniciar el proceso.");
        }
    }

    // Otras funciones como asignar variables y completar tareas...
}