using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Text;

namespace Backend.Services
{
    public class BonitaService
    {
        private readonly HttpClient _httpClient;
        private string _token;

        public BonitaService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> LoginAsync(string username, string password)
        {
            var loginUrl = "http://localhost:8080/bonita/loginservice";  // URL de login de Bonita
            //var loginUrl = "http://localhost:29810/bonita/loginservice";  // URL de login de Bonita

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

        public void SetToken(string token) {
            _token = token;
        }

        public async Task<string> GetProcessIdAsync(string processName) {
            _httpClient.DefaultRequestHeaders.Add("X-Bonita-API-Token", _token);
            try {

                var response = await _httpClient.GetAsync($"http://localhost:8080/bonita/API/bpm/process?s={processName}");
                //var response = await _httpClient.GetAsync($"http://localhost:29810/bonita/API/bpm/process?s={processName}");
                string responseBody = await response.Content.ReadAsStringAsync();
                Console.WriteLine(responseBody);
                JArray processes = JArray.Parse(responseBody);
                if (processes.Count > 0) {
                    var processId = processes[0]["id"].ToString();
                    return processId;
                }
            } catch (System.Exception ex) {
                Console.WriteLine($"Ocurrió un error al recuperar el proceso: {ex.Message}");
            }

            return null;
        }

        public async Task<string> CompletarActividadAsync(string caseId)
        {
            try
            {
                // Obtenemos la siguiente tarea del proceso
                var taskId = await GetNextTaskAsync(caseId);

                if (taskId.Contains("No hay tareas pendientes"))
                {
                    return $"No se encontró ninguna tarea para el caseId: {caseId}";
                }

                // Ejecutamos la tarea con el taskId
                var resultado = await ExecuteTaskAsync(taskId);

                return resultado;  // Retornamos el resultado de la ejecución de la tarea
            }
            catch (Exception ex)
            {
                return $"Error al completar la actividad: {ex.Message}";
            }
        }

        public async Task<string> ExecuteTaskAsync(string taskId)
        {
             try
                {
                    _httpClient.DefaultRequestHeaders.Add("X-Bonita-API-Token", _token);

                    // Usamos interpolación de strings para asegurarnos de que el taskId esté correctamente en la URL
                    //var taskUrl = $"http://localhost:29810/bonita/API/bpm/userTask/{taskId}/execution";
                    var taskUrl = $"http://localhost:8080/bonita/API/bpm/userTask/{taskId}/execution";

                    // Realizamos la solicitud POST para completar la tarea
                    var response = await _httpClient.PostAsync(taskUrl, null);  // Enviamos una solicitud vacía

                    if (!response.IsSuccessStatusCode)
                    {
                        return $"Error al completar la tarea: {response.ReasonPhrase}";
                    }

                    // Leemos el contenido de la respuesta
                    string responseBody = await response.Content.ReadAsStringAsync();
                    return $"Tarea completada con éxito. Respuesta: {responseBody}";
                }
                catch (System.Exception ex)
                {
                    Console.WriteLine($"Ocurrió un error: {ex.Message}");
                    return "No se pudo finalizar la tarea";
                }


        }



        public async Task<string> StartProcessAsync(string processDefinitionId) {
            _httpClient.DefaultRequestHeaders.Add("X-Bonita-API-Token", _token);

            // Crear la solicitud en formato JSON
            var bodyObject = new
            {
                processDefinitionId = processDefinitionId
            };

            // Serialize the object to JSON
            var jsonContent = new StringContent(JsonConvert.SerializeObject(bodyObject), Encoding.UTF8, "application/json");

            // Make the POST request to start the process instance
            var response = await _httpClient.PostAsync("http://localhost:8080/bonita/API/bpm/case", jsonContent);
            //var response = await _httpClient.PostAsync("http://localhost:29810/bonita/API/bpm/case", jsonContent);

            // Leer el contenido de la respuesta
            string responseBody = await response.Content.ReadAsStringAsync();

            var jsonResponse = JObject.Parse(responseBody);

            var caseId = jsonResponse["id"]?.ToString();

            // Devolver la respuesta (el ID de la instancia del proceso que se inició)
            return caseId ?? "No se pudo obtener el ID del caso";
        }

        public async Task<string> GetNextTaskAsync(string caseId)
        {
            _httpClient.DefaultRequestHeaders.Add("X-Bonita-API-Token", _token);

            // Realiza la solicitud GET para obtener las tareas pendientes del proceso
            //var response = await _httpClient.GetAsync($"http://localhost:29810/bonita/API/bpm/task?f=caseId={caseId}");
            var response = await _httpClient.GetAsync($"http://localhost:8080/bonita/API/bpm/task?f=caseId={caseId}");

            if (!response.IsSuccessStatusCode)
            {
                return $"Error al obtener la próxima tarea: {response.ReasonPhrase}";
            }

            // Leer el contenido de la respuesta
            string responseBody = await response.Content.ReadAsStringAsync();

            // Parsear la respuesta JSON
            var jsonResponse = JArray.Parse(responseBody);

            // Asumimos que la primera tarea disponible es la siguiente
            var nextTaskId = jsonResponse.First?["id"]?.ToString();

            return nextTaskId ?? "No hay tareas pendientes para este caso";
        }

        public async Task<string> GetUserIdAsync(string userName) {
            _httpClient.DefaultRequestHeaders.Add("X-Bonita-API-Token", _token);
            var response = await _httpClient.GetAsync($"http://localhost:29810/bonita/API/identity/user?p=0&c=10&f=userName={userName}");
             //var response = await _httpClient.GetAsync($"http://localhost:29810/bonita/API/identity/user?p=0&c=10&f=userName={userName}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var jsonArray = JArray.Parse(content);

                // Extrae el ID del usuario desde el primer resultado (si existe)
                if (jsonArray.Any())
                {
                    var userId = jsonArray[0]["id"].ToString();
                    return userId;
                }
            }
            return null;
        }

        public async Task<bool> AssignTaskToUserAsync(string taskId, string userId)
        {
            try {
                _httpClient.DefaultRequestHeaders.Add("X-Bonita-API-Token", _token);

                // Crear la solicitud en formato JSON
                var bodyObject = new
                {
                    assigned_id = userId
                };

                // Serialize the object to JSON
                var jsonContent = new StringContent(JsonConvert.SerializeObject(bodyObject), Encoding.UTF8, "application/json");

                // Make the POST request to start the process instance
                //var response = await _httpClient.PostAsync("http://localhost:8080/bonita/API/bpm/case", jsonContent);
                var response = await _httpClient.PutAsync($"http://localhost:29810/bonita/API/bpm/humanTask/{taskId}", jsonContent);

                // Retorna true si la solicitud fue exitosa
                return response.IsSuccessStatusCode;
            } catch (System.Exception ex) {
                Console.WriteLine("Ocurrió un error al asignar la tarea al usuario: {ex.message}");
                return false;
            }


        }

        /*public async Task ExecuteTaskAsync(string taskId, string token) {
            _httpClient.DefaultResquestHeaders.Add("X-Bonita-API-Token", token);
            var taskUrl = "http://localhost:8080/bonita/API/bpm/userTask/";
            var content = new StringContent("");
            var response = await _httpClient($"{")
        }*/
    }
}
