using Backend.Dto;
using Backend.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;


namespace api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BonitaController : ControllerBase
    {
        private readonly BonitaService _bonitaService;


        public BonitaController(BonitaService bonitaService)
        {
            _bonitaService = bonitaService;
        }

        private IActionResult CheckToken(string token) {
            if (string.IsNullOrEmpty(token)) {
                return Unauthorized(new { message = "Unauthorized: X-Bonita-API-Token is required" });
            }
            return null;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] BonitaLoginRequest request)
        {
            try
            {
                var token = await _bonitaService.LoginAsync(request.username, request.password);
                if (token != null)
                {
                    // Retornamos el token al frontend
                    return Ok(new { Token = token });
                }
                else
                {
                    return Unauthorized("No se pudo obtener el token de Bonita.");
                }
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("logout")]
        public async Task<IActionResult> Logout([FromHeader(Name = "X-Bonita-API-Token")] string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized(new { message = "Missing X-Bonita-API-Token" });
            }

            var result = await _bonitaService.LogoutAsync(token);

            if (result)
            {
                return Ok(new { message = "Logout successful" });
            }
            else
            {
                return StatusCode(500, new { message = "Logout failed" });
            }
        }

        [HttpGet("process/{processName}")]
        public async Task<IActionResult> getProcessId(string processName, [FromHeader(Name = "X-Bonita-API-Token")] string token)
        {
            Console.WriteLine($"{processName}");
            var tokenCheck = CheckToken(token);
            if (tokenCheck != null) {
                return tokenCheck;
            }
            try {
                var processId = await _bonitaService.GetProcessIdAsync(processName, token);
                if (processId != null) {
                    return Ok(new { processId });
                } else {
                    return NotFound("Process not found");
                }
            } catch (System.Exception ex) {
                return StatusCode(500, ex.Message);
            }

        }

        [HttpGet("startprocess/{processId}")]
        public async Task<IActionResult> startProcessById(string processId, [FromHeader(Name = "X-Bonita-API-Token")] string token)
        {
            var tokenCheck = CheckToken(token);
            if (tokenCheck != null) {
                return tokenCheck;
            }

            try {
                var processInstance = await _bonitaService.StartProcessAsync(processId, token);

                return Ok(new { processInstance });
            } catch (System.Exception ex) {
                return StatusCode(500, new { message = ex.Message });
            }
        }



        [HttpGet("{username}/id")]
        public async Task<IActionResult> GetUserId(string username, [FromHeader(Name = "X-Bonita-API-Token")] string token){
            try {
                var tokenCheck = CheckToken(token);
                if (tokenCheck != null) {
                    return tokenCheck;
                }

                var userId = await _bonitaService.GetUserIdAsync(username, token);
                if (userId != null) {
                    return Ok(new { userId });
                }
                return NotFound("Usuario no encontrado");
            } catch (System.Exception ex) {
                return StatusCode(500, new { message = ex.Message });
            }

        }

        [HttpGet("getNextTask/{caseId}")]
        public async Task<IActionResult> nextTaskByCaseId(string caseId, [FromHeader(Name = "X-Bonita-API-Token")] string token)
        {
            var tokenCheck = CheckToken(token);
            if (tokenCheck != null) {
                return tokenCheck;
            }

            try {
                var nextTaskId = await _bonitaService.GetNextTaskAsync(caseId, token);

                return Ok(new { nextTaskId });
            } catch (System.Exception ex) {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpGet("task/{taskId}")]
        public async Task<IActionResult> GetTaskInfo(string taskId, [FromHeader(Name = "X-Bonita-API-Token")] string token)
        {
            var tokenCheck = CheckToken(token);
            if (tokenCheck != null)
            {
                return tokenCheck;
            }

            try
            {
                var taskInfo = await _bonitaService.GetTaskInfoAsync(taskId, token);

                if (taskInfo.ContainsKey("status") && taskInfo["status"].ToString() == "401")
                {
                    return Unauthorized(new { message = taskInfo["message"] });
                }

                if (taskInfo.ContainsKey("status") && taskInfo["status"].ToString() == "404")
                {
                    return NotFound(new { message = taskInfo["message"] });
                }

                if (taskInfo.ContainsKey("status") && taskInfo["status"].ToString() == "500")
                {
                    return StatusCode(500, new { message = taskInfo["message"] });
                }

                if (taskInfo.ContainsKey("data"))
                {
                    var data = taskInfo["data"];

                    var taskDetails = new
                    {
                        DisplayDescription = data?["displayDescription"]?.ToString(),
                        ExecutedBy = data?["executedBy"]?.ToString(),
                        RootContainerId = data?["rootContainerId"]?.ToString(),
                        AssignedDate = data?["assigned_date"]?.ToString(),
                        DisplayName = data?["displayName"]?.ToString(),
                        ExecutedBySubstitute = data?["executedBySubstitute"]?.ToString(),
                        DueDate = data?["dueDate"]?.ToString(),
                        Description = data?["description"]?.ToString(),
                        Type = data?["type"]?.ToString(),
                        Priority = data?["priority"]?.ToString(),
                        ActorId = data?["actorId"]?.ToString(),
                        ProcessId = data?["processId"]?.ToString(),
                        CaseId = data?["caseId"]?.ToString(),
                        Name = data?["name"]?.ToString(),
                        ReachedStateDate = data?["reached_state_date"]?.ToString(),
                        RootCaseId = data?["rootCaseId"]?.ToString(),
                        Id = data?["id"]?.ToString(),
                        State = data?["state"]?.ToString(),
                        ParentCaseId = data?["parentCaseId"]?.ToString(),
                        LastUpdateDate = data?["last_update_date"]?.ToString(),
                        AssignedId = data?["assigned_id"]?.ToString()
                    };

                    return Ok(taskDetails);
                }

                return StatusCode(500, new { message = "Unexpected error occurred." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }


        [HttpPut("task/{taskId}/assign/{userId}")]
        public async Task<IActionResult> AssignTaskToUser(string taskId, string userId, [FromHeader(Name = "X-Bonita-API-Token")] string token)
        {
            // Check if token is provided
            var tokenCheck = CheckToken(token);
            if (tokenCheck != null)
            {
                return tokenCheck; // Return 401 if token is missing
            }

            try
            {
                // Call the service to assign the task
                var result = await _bonitaService.AssignTaskToUserAsync(taskId, userId, token);

                // Handle response based on the returned structure
                if (result.ContainsKey("status") && result["status"].ToString() == "401")
                {
                    return Unauthorized(new { message = result["message"] });
                }

                if (result.ContainsKey("status") && result["status"].ToString() == "404")
                {
                    return NotFound(new { message = result["message"].ToString() });
                }

                if (result.ContainsKey("status") && result["status"].ToString() == "500")
                {
                    return StatusCode(500, new { message = result["message"].ToString() });
                }

                if (result.ContainsKey("ok") && (bool)result["ok"])
                {
                    return Ok(new { message = result["message"].ToString() });
                }

                // Fallback in case of unexpected response
                return StatusCode(500, new { message = "Unexpected error occurred." });
            }
            catch (Exception ex)
            {
                // Handle any unhandled errors
                return StatusCode(500, new { message = ex.Message });
            }

        }

        [HttpPut("case/{caseId}/variable/{variableName}")]
        public async Task<IActionResult> SetVariableValue(
            string caseId,
            string variableName,
            [FromHeader(Name = "X-Bonita-API-Token")] string token,
            [FromBody] VariableRequest variableRequest)
        {
            // Check if token is provided
            var tokenCheck = CheckToken(token);
            if (tokenCheck != null)
            {
                return tokenCheck; // Return 401 if token is missing
            }

            var booleano = variableRequest.Value.ToString().ToLower() == "true" ? true : false;
            Console.WriteLine(booleano);
            try
            {
                // Pass the data to the service
                var result = await _bonitaService.SetVariableValueAsync(caseId, variableName, booleano, token);

                // Handle response from service
                if (result.ContainsKey("status") && result["status"].ToString() == "401")
                {
                    return Unauthorized(new { message = result["message"].ToString() });
                }

                if (result.ContainsKey("status") && result["status"].ToString() == "404")
                {
                    return NotFound(new { message = result["message"].ToString() });
                }

                if (result.ContainsKey("status") && result["status"].ToString() == "500")
                {
                    return StatusCode(500, new { message = result["message"].ToString() });
                }

                if (result.ContainsKey("ok") && (bool)result["ok"])
                {
                    return Ok(new { message = result["message"].ToString() });
                }

                // Fallback for unexpected responses
                return StatusCode(500, new { message = "Unexpected error occurred." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }


        [HttpPost("task/{taskId}/execute")]
        public async Task<IActionResult> ExecuteTask(string taskId, [FromHeader(Name = "X-Bonita-API-Token")] string token)
        {
            // Check if token is provided
            var tokenCheck = CheckToken(token);
            if (tokenCheck != null)
            {
                return tokenCheck; // Return 401 if token is missing
            }

            try
            {
                // Call the service to execute the task
                var result = await _bonitaService.ExecuteTaskAsync(taskId, token);

                // Handle response based on the returned structure
                if (result.ContainsKey("status") && result["status"].ToString() == "401")
                {
                    return Unauthorized(new { message = result["message"].ToString() });
                }

                if (result.ContainsKey("status") && result["status"].ToString() == "404")
                {
                    return NotFound(new { message = result["message"].ToString() });
                }

                if (result.ContainsKey("status") && result["status"].ToString() == "500")
                {
                    return StatusCode(500, new { message = result["message"].ToString() });
                }

                if (result.ContainsKey("ok") && (bool)result["ok"])
                {
                    return Ok(new { message = result["message"].ToString() });
                }

                // Fallback in case of unexpected response
                return StatusCode(500, new { message = "Unexpected error occurred." });
            }
            catch (Exception ex)
            {
                // Handle any unhandled errors
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpGet("case/{caseId}/variable/{variableName}")]
        public async Task<IActionResult> GetVariableValue(
            string caseId,
            string variableName,
            [FromHeader(Name = "X-Bonita-API-Token")] string token)
        {
            // Check if the token is provided
            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized(new { message = "Unauthorized: X-Bonita-API-Token is required" });
            }

            try
            {
                // Call the service to get the variable value
                var result = await _bonitaService.GetVariableValueAsync(caseId, variableName, token);

                if (result.ContainsKey("status") && result["status"].ToString() == "401")
                {
                    return Unauthorized(new { message = result["message"] });
                }

                if (result.ContainsKey("status") && result["status"].ToString() == "404")
                {
                    return NotFound(new { message = result["message"] });
                }

                if (result.ContainsKey("data"))
                {
                    var data = result["data"];

                    var variableDetails = new
                    {
                        CaseId = data?["case_id"]?.ToString(),
                        Name = data?["name"]?.ToString(),
                        Description = data?["description"]?.ToString(),
                        Type = data?["type"]?.ToString(),
                        Value = data?["value"]?.ToString(),
                    };

                    return Ok(variableDetails);
                }

                // Handle unexpected responses
                return StatusCode(500, new { message = "Unexpected error occurred." });
            }
            catch (Exception ex)
            {
                // Handle any unhandled errors
                return StatusCode(500, new { message = ex.Message });
            }
        }

    }


    // DTO para el login
    public class BonitaLoginRequest
    {
        public string username { get; set; }
        public string password { get; set; }
    }

    public class VariableValueDto
    {
        public string Type { get; set; }
        public object Value { get; set; }
    }

}
