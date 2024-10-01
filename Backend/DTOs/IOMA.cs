namespace api.Dto;

public class IOMAAuthModel
{
    public string grant_type { get; set; }
    public string scope { get; set; }
    public string username { get; set; }
    public string client_id { get; set; }
    public string client_secret { get; set; }

    public IOMAAuthModel(string username, string client_id, string client_secret)
    {
        this.grant_type = "client_credentials";
        this.scope = "roles openid profile APIAutorizaciones.Medicas.Consulta APIConsumos.Medicos.Read APIAutorizaciones.Medicas.Practica OrdenMedica RecetaElectronica APIPrestacionesDigitales.EntidadesProforma";
        this.username = username;
        this.client_id = client_id;
        this.client_secret = client_secret;
    }
}

public class IOMAValidacionDNIModel {
    public int sexo { get; set; }
    public int documento { get; set; }
    public int token { get; set; }
    public string nroSolicitud { get; set; }

    public IOMAValidacionDNIModel(string sexo, string documento, string token, string nroSolicitud)
    {
        this.sexo = int.Parse(sexo);
        this.documento = int.Parse(documento);
        this.token = int.Parse(token);
        this.nroSolicitud = nroSolicitud;
    }
}

public class IOMAValidacionNroAfiliadoModel {
    public string nroAfiliado { get; set; }
    public int token { get; set; }
    public string nroSolicitud { get; set; }

    public IOMAValidacionNroAfiliadoModel(string nroAfiliado, string token, string nroSolicitud)
    {
        this.nroAfiliado = nroAfiliado;
        this.token = int.Parse(token);
        this.nroSolicitud = nroSolicitud;
    }
}

public class AuthResponseModel
{
    public required string access_token { get; set; }
    public required int expires_in { get; set; }
    public required string token_type { get; set; }
    public required string scope { get; set; }
}

public class ValidarPacienteResponse
{
    public required string codigoMensaje { get; set; }
    public required string mensaje { get; set; }
    public required string nroSolicitud { get; set; }
    public required int nroTransaccion { get; set; }
    public required string numeroAfiliado { get; set; }
    public required string apellidoNombre { get; set; }
    public required string cuil { get; set; }
    public required int numeroDocumento { get; set; }
    public required int tipoDocumento { get; set; }
    public required string tipoDocumentoDescripcion { get; set; }
    public required int sexo { get; set; }
    public required string fechaNacimiento { get; set; }
    public required string periodo { get; set; }
    public required string fechaIngreso { get; set; }
    public required string fechaCese { get; set; }
    public required string codigoBaja { get; set; }
    public required string tipoAfiliatorio { get; set; }
    public required string localidad { get; set; }
    public required string localidadDescripcion { get; set; }
    public required string partido { get; set; }
    public required string partidoDescripcion { get; set; }
}

public class ErrorResponse
{
    public required int nroSolicitud { get; set; }
    public required int nroTransaccion { get; set; }
    public required string codigoMensaje { get; set; }
    public required string mensaje { get; set; }
}