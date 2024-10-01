namespace api.Dto;

public class ValidarPaciente
{
    public required string ObraSocial { get; set; }
    public required string TipoDocumento { get; set; }
    public required string NumeroDocumento { get; set; }
    public required string Sexo { get; set; }
    public required string Token { get; set; }
    public required string Username { get; set; }
}

public class PacienteResultadoValidacion {
    public string apellidoNombre { get; set; }
    public string numeroAfiliado { get; set; }
    public string tipoAfiliatorio { get; set; }
    public string partidoDescripcion { get; set; }
    public string localidadDescripcion { get; set; }
    public int nroTransaccion { get; set; }
    public Guid id { get; set; }
    public string? mail { get; set; }
    public string? telefono { get; set; }
    public string? direccion { get; set; }

    public PacienteResultadoValidacion(string apellidoNombre, string numeroAfiliado, string tipoAfiliatorio,
                                       string partidoDescripcion, string localidadDescripcion,
                                       int nroTransaccion, Guid id, string? mail,
                                       string? telefono, string? direccion)
    {
        this.apellidoNombre = apellidoNombre;
        this.numeroAfiliado = numeroAfiliado;
        this.tipoAfiliatorio = tipoAfiliatorio;
        this.partidoDescripcion = partidoDescripcion;
        this.localidadDescripcion = localidadDescripcion;
        this.nroTransaccion = nroTransaccion;
        this.id = id;
        this.mail = mail != null ? mail : null;
        this.telefono = telefono != null ? telefono : null;
        this.direccion = direccion != null ? direccion : null;
    }
}
public class PacienteDatosAdicionales
{
    public Guid PacienteId { get; set; }
    public string? Mail { get; set; }
    public string? Telefono { get; set; }
    public string? Direccion { get; set; }
}