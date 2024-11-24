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

        public BonitaService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> LoginAsync(string username, string password)
        {
            //var loginUrl = "http://localhost:8080/bonita/loginservice";  // URL de login de Bonita
            var loginUrl = "http://localhost:29810/bonita/loginservice";  // URL de login de Bonita

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

        public async Task<bool> LogoutAsync(string token)
        {
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("X-Bonita-API-Token", token);

            try
            {
                // Send a DELETE request to Bonita's session endpoint to log out
                var response = await _httpClient.GetAsync("http://localhost:29810/bonita/logoutservice");

                // Check if the response was successful
                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Logout successful");
                    return true;
                }
                else
                {
                    Console.WriteLine("Failed to log out. Status Code: " + response.StatusCode);
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error during logout: " + ex.Message);
                return false;
            }
        }

        public async Task<string> GetProcessIdAsync(string processName, string token) {
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("X-Bonita-API-Token", token);
            try {
                Console.WriteLine(_httpClient.DefaultRequestHeaders);
                //var response = await _httpClient.GetAsync($"http://localhost:8080/bonita/API/bpm/process?s={processName}");
                var response = await _httpClient.GetAsync($"http://localhost:29810/bonita/API/bpm/process?s={processName}");

                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return "El token es inválido";
                }

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

        public async Task<string> StartProcessAsync(string processDefinitionId, string token)
        {
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("X-Bonita-API-Token", token);

            // Create the request body as JSON
            var bodyObject = new
            {
                processDefinitionId = processDefinitionId,
            };

            try
            {
                // Serialize the object to JSON
                var jsonContent = new StringContent(JsonConvert.SerializeObject(bodyObject), Encoding.UTF8, "application/json");

                // Make the POST request to start the process instance
                var response = await _httpClient.PostAsync("http://localhost:29810/bonita/API/bpm/case", jsonContent);
                string responseBody = await response.Content.ReadAsStringAsync();
                Console.WriteLine(responseBody);
                // Check if the response status is 401
                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return "El token es inválido";
                }


                // Parse the response to get the case ID
                var jsonResponse = JObject.Parse(responseBody);
                var caseId = jsonResponse["id"]?.ToString();

                // Return the case ID or a message if it couldn't be retrieved
                return caseId ?? "No se pudo obtener el ID del caso";
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                Console.WriteLine("Error starting process: " + ex.Message);
                return "Ocurrió un error al intentar iniciar el proceso";
            }
        }

        public async Task<string> GetNextTaskAsync(string caseId, string token)
        {
            _httpClient.DefaultRequestHeaders.Add("X-Bonita-API-Token", token);
            Console.WriteLine(caseId);
            // Realiza la solicitud GET para obtener las tareas pendientes del proceso
            var response = await _httpClient.GetAsync($"http://localhost:29810/bonita/API/bpm/task?f=caseId={caseId}");
            //var response = await _httpClient.GetAsync($"http://localhost:8080/bonita/API/bpm/task?f=caseId={caseId}");

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                return "El token es inválido";
            }

            if (!response.IsSuccessStatusCode)
            {
                return $"Error al obtener la próxima tarea: {response.ReasonPhrase}";
            }

            // Leer el contenido de la respuesta
            string responseBody = await response.Content.ReadAsStringAsync();

            // Parsear la respuesta JSON
            var jsonResponse = JArray.Parse(responseBody);
            Console.WriteLine(jsonResponse);
            // Asumimos que la primera tarea disponible es la siguiente
            var nextTaskId = jsonResponse.First?["id"]?.ToString();

            return nextTaskId ?? "No hay tareas pendientes para este caso";
        }

        public async Task<string> GetUserIdAsync(string userName, string token) {
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("X-Bonita-API-Token", token);
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
            } else {
                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return "El token es inválido";
                } else {
                    return "Ocurrió un error al recuperar el ID del usuario";
                }
            }
            return null;
        }

        public async Task<JObject> GetTaskInfoAsync(string taskId, string token)
        {
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("X-Bonita-API-Token", token);

            try
            {
                // Make the request to Bonita API to get task information
                var response = await _httpClient.GetAsync($"http://localhost:29810/bonita/API/bpm/task/{taskId}");

                // Check for specific response statuses
                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return JObject.FromObject(new { status = "401", message = "El token es inválido" });
                }

                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return JObject.FromObject(new { status = "404", message = "Tarea no encontrada" });
                }

                if (response.IsSuccessStatusCode)
                {
                    // Parse and return task data if successful
                    var responseBody = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(responseBody);
                    return JObject.FromObject(new { status = "200", data = JObject.Parse(responseBody) });
                }

                // Handle other unexpected status codes
                return JObject.FromObject(new { status = "500", message = "Ocurrió un error al recuperar la tarea" });
            }
            catch (Exception ex)
            {
                // Catch any exceptions and return a 500 status
                Console.WriteLine($"Error retrieving task info: {ex.Message}");
                return JObject.FromObject(new { status = "500", message = "Ocurrió un error interno" });
            }
        }

        public async Task<JObject> AssignTaskToUserAsync(string taskId, string userId, string token)
        {
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("X-Bonita-API-Token", token);

            try
            {
                // Create the request payload
                var bodyObject = new { assigned_id = userId };
                var jsonContent = new StringContent(JsonConvert.SerializeObject(bodyObject), Encoding.UTF8, "application/json");

                // Make the PUT request to assign the task
                var response = await _httpClient.PutAsync($"http://localhost:29810/bonita/API/bpm/userTask/{taskId}", jsonContent);

                // Check for specific response statuses
                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return JObject.FromObject(new { status = "401", message = "El token es inválido" });
                }

                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return JObject.FromObject(new { status = "404", message = "Tarea no encontrada" });
                }

                if (response.IsSuccessStatusCode)
                {
                    // Task assignment successful
                    return JObject.FromObject(new { ok = true, message = "Tarea asignada correctamente" });
                }

                // Handle unexpected status codes
                return JObject.FromObject(new { status = "500", message = "Ocurrió un error al asignar la tarea" });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error assigning task to user: {ex.Message}");
                return JObject.FromObject(new { status = "500", message = "Ocurrió un error interno" });
            }
        }

        public async Task<JObject> SetVariableValueAsync(string caseId, string variableName, bool booleano, string token)
        {
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("X-Bonita-API-Token", token);

            try
            {
                // Serialize the request body
                var hardcodeado = new {
                    type = "java.lang.Boolean",
                    value = booleano ? "true" : "false"
                };
                var jsonContent = new StringContent(JsonConvert.SerializeObject(hardcodeado), Encoding.UTF8, "application/json");
                Console.WriteLine("jsonContent: " + jsonContent);
                // Make the PUT request to Bonita
                var response = await _httpClient.PutAsync($"http://localhost:29810/bonita/API/bpm/caseVariable/{caseId}/{variableName}", jsonContent);

                // Handle the response
                if (response.IsSuccessStatusCode)
                {
                    return JObject.FromObject(new { ok = true, message = "Variable value set successfully." });
                }

                // Parse error response
                var responseBody = await response.Content.ReadAsStringAsync();
                return JObject.FromObject(new { status = (int)response.StatusCode, message = responseBody });
            }
            catch (Exception ex)
            {
                return JObject.FromObject(new { status = 500, message = ex.Message });
            }
        }


        public async Task<JObject> ExecuteTaskAsync(string taskId, string token)
        {
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("X-Bonita-API-Token", token);

            try
            {
                // Create the request payload for execution (if required)
                var bodyObject = new { };
                var jsonContent = new StringContent(JsonConvert.SerializeObject(bodyObject), Encoding.UTF8, "application/json");

                // Make the POST request to execute the task
                var response = await _httpClient.PostAsync($"http://localhost:29810/bonita/API/bpm/userTask/{taskId}/execution", jsonContent);

                // Check for specific response statuses
                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return JObject.FromObject(new { status = "401", message = "El token es inválido" });
                }

                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return JObject.FromObject(new { status = "404", message = "Tarea no encontrada" });
                }

                if (response.IsSuccessStatusCode)
                {
                    // Task executed successfully
                    return JObject.FromObject(new { ok = true, message = "Tarea ejecutada correctamente" });
                }

                // Handle unexpected status codes
                return JObject.FromObject(new { status = "500", message = "Ocurrió un error al ejecutar la tarea" });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error executing task: {ex.Message}");
                return JObject.FromObject(new { status = "500", message = "Ocurrió un error interno" });
            }
        }

        public async Task<JObject> GetVariableValueAsync(string caseId, string variableName, string token)
        {
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("X-Bonita-API-Token", token);

            try
            {
                // Make the GET request to retrieve the variable value
                var response = await _httpClient.GetAsync($"http://localhost:29810/bonita/API/bpm/caseVariable/{caseId}/{variableName}");

                // Handle specific status codes
                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return JObject.FromObject(new { status = "401", message = "Unauthorized: Invalid token." });
                }

                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return JObject.FromObject(new { status = "404", message = $"Variable '{variableName}' not found in case '{caseId}'." });
                }

                if (response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    var variableData = JObject.Parse(responseBody);
                    Console.WriteLine(variableData);
                    return JObject.FromObject(new { data = variableData });
                }

                // Handle unexpected responses
                return JObject.FromObject(new { status = "500", message = "Unexpected error occurred while retrieving the variable value." });
            }
            catch (Exception ex)
            {
                // Handle exceptions
                return JObject.FromObject(new { status = "500", message = $"Error: {ex.Message}" });
            }
        }

    }

    public class VariableRequest
    {
        public string Type { get; set; } // e.g., "java.lang.Boolean"
        public object Value { get; set; } // e.g., true or false
    }


}
