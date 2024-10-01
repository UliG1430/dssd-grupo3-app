namespace api.Dto;

public class FacturadorModel
{
    public required string nombre { get; set; }
    public required string direccion { get; set; }
    public required string cuitPrimario { get; set; }
    public required string cuitFacturador { get; set; }
    public required bool esEstablecimiento { get; set; }
}

public class ProfesionalFacturadorModel
{
    public required Guid idProfesional { get; set; }
    public required Guid idFacturador { get; set; }
}

public class GetProfesionalesFacturadorBody
{
    public required Guid idFacturador { get; set; }
}