using System.Net.Http.Headers;
using api.Dto;
using ApiACEAPP.Helpers;
using System.Text.Json;
using ApiACEAPP.Repositories;
using api.Model;
using Microsoft.AspNetCore.Mvc;

namespace ApiACEAPP.Services;
public class IOMAService
{    
    private readonly IConfiguration _configuration;
    private readonly AuthIOMARepository _authIOMARepository;
    private readonly ValidacionRepository _validacionRepository;

    public IOMAService(IConfiguration configuration, AuthIOMARepository authIOMARepository, ValidacionRepository validacionRepository)
    {
        _configuration = configuration;
        _authIOMARepository = authIOMARepository;
        _validacionRepository = validacionRepository;
    }

    private async Task<string> GetNewTokenAuth()
    {
        string url = _configuration["URLs:IOMA:Auth"];

        var parameters = new Dictionary<string, string>
        {
            { "grant_type", _configuration["IOMACredentials:GrantType"] },
            { "password", _configuration["IOMACredentials:Password"] },
            { "scope", _configuration["IOMACredentials:Scope"] },
            { "username", _configuration["IOMACredentials:Username"] },
            { "client_id", _configuration["IOMACredentials:ClientId"] },
            { "client_secret", _configuration["IOMACredentials:ClientSecret"] }
        };
        try
        {
            var result = new HttpResponseMessage();
            var response = string.Empty;
            AuthResponseModel authResponseObj = null;
            using (HttpClient client = new HttpClient())
			{
				client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

				System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
				
                result = await client.PostAsync(url, new FormUrlEncodedContent(parameters));
                response = await result.Content.ReadAsStringAsync();
                authResponseObj = JsonSerializer.Deserialize<AuthResponseModel>(response);
			}

            AuthIOMA authIOMA = new AuthIOMA(){
                Id = Guid.NewGuid(),
                FechaSolicitud = DateTime.UtcNow,
                AccessToken = authResponseObj.access_token,
                ExpiresIn = authResponseObj.expires_in,
                TokenType = authResponseObj.token_type,
                Scope = authResponseObj.scope
            };
            await _authIOMARepository.AddAsync(authIOMA);

            return authResponseObj.access_token;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        finally
        {
            //_logger.LogInformation("GetNewTokenAuth");
        }
    }

    private ValidarPacienteResponse ParsearTipoAfiliatorio(ValidarPacienteResponse objResponse)
    {
        switch (objResponse.tipoAfiliatorio) {
            case "O":
                objResponse.tipoAfiliatorio = "Obligatorio";
                break;
            case "V":
                objResponse.tipoAfiliatorio = "Voluntario";
                break;
            case "X":
                objResponse.tipoAfiliatorio = "Extr. Jurisdicción";
                break;
            case "B":
                objResponse.tipoAfiliatorio = "Beneficiario";
                break;
            case "C":
                objResponse.tipoAfiliatorio = "Colectivo";
                break;
        }
        return objResponse;
    }

    public async Task<string> GetTokenAuth()
    {
        try
        {
            var tokenVigente = (await _authIOMARepository.FilterAsync(x => x.FechaSolicitud >= DateTime.UtcNow.AddMinutes(-55))).FirstOrDefault();

            if (tokenVigente == null)
            {
                return await GetNewTokenAuth();
            }
            else
            {
                return tokenVigente.AccessToken;
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        finally
        {
            //_logger.LogInformation("GetTokenAuth");
        }
    }

    public async Task<ServiceResult<ValidarPacienteResponse>> ValidarPaciente(ValidarPaciente validarPaciente)
    {
        var url = _configuration["URLs:IOMA:Validacion"];
        string nroValidacion = "0"; //CHECK?!?!?!?!?!
        var body = new object();
        
        if (validarPaciente.TipoDocumento == "Carnet de Afiliado")
        {
            body = new IOMAValidacionNroAfiliadoModel(validarPaciente.NumeroDocumento,
                                                      validarPaciente.Token,
                                                      nroValidacion);
        }
        else
        {
            body = new IOMAValidacionDNIModel(validarPaciente.Sexo,
                                              validarPaciente.NumeroDocumento,
                                              validarPaciente.Token,
                                              nroValidacion);
        }
        
        string response = "";
        Validacion validacion;
        ServiceResult<ValidarPacienteResponse> result;
        try
        {
            string token = await GetTokenAuth();
            response = await HttpClientHelper.SendRequest(url, "POST", body, token);
            ValidarPacienteResponse? objResponse = JsonSerializer.Deserialize<ValidarPacienteResponse>(response);

            objResponse = ParsearTipoAfiliatorio(objResponse);

            validacion = new Validacion()
            {
                Id = Guid.NewGuid(),
                Token = validarPaciente.Token,
                Fecha = DateTime.UtcNow,
                JSONresponse = response,
                NroTransaccion = objResponse.nroTransaccion,
                NroAfiliado = objResponse.numeroAfiliado,
                TipoAfiliatorio = objResponse.tipoAfiliatorio,
                NombreApellidoAfiliado = objResponse.apellidoNombre,
                Username = validarPaciente.Username
            };
            await _validacionRepository.AddAsync(validacion);

            result = new ServiceResult<ValidarPacienteResponse>
            {
                Data = objResponse,
                ErrorMessage = "",
                Success = true
            };
        }
        catch (JsonException ex)
        {
            ErrorResponse? errorResponse = JsonSerializer.Deserialize<ErrorResponse>(response);
            validacion = new Validacion()
            {
                Id = Guid.NewGuid(),
                Token = validarPaciente.Token,
                Fecha = DateTime.UtcNow,
                JSONresponse = response,
                NroTransaccion = errorResponse.nroTransaccion,
                Username = validarPaciente.Username
            };
            await _validacionRepository.AddAsync(validacion);

            result = new ServiceResult<ValidarPacienteResponse>
            {
                Data = null,
                ErrorMessage = errorResponse.mensaje,
                Success = false
            };
        }
        catch (Exception ex)
        {
            result = new ServiceResult<ValidarPacienteResponse>
            {
                Data = null,
                ErrorMessage = ex.Message,
                Success = false
            };
        }
        finally
        {
            //_logger.LogInformation("ValidarPaciente");
        }
        return result;
    }
}