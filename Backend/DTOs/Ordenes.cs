using api.Model;

namespace api.Dto;

public class PracticaModel
{
    public required string codigo { get; set; }
    public required string descripcion { get; set; }
    public required string tipo { get; set; }
    public required bool activa { get; set; }
    public required float monto { get; set; }
}

public class PracticaASolicitar
{
    public required string Codigo { get; set; }
    public required Guid IdDiagnostico { get; set; }
    public required int Cantidad { get; set; }
}

public class GetSolicitudesFacturadorModel
{
    public required Guid idFacturador { get; set; }
}

public class DiagnosticosModel
{
    public required string Codigo { get; set; }
    public required string Descripcion { get; set; }
    public required string Patologia { get; set; }
}

public class SolicitudModel
{
    public required string MatriculaEfector {get;set;}
    public required string NombreApellidoEfector {get;set;}
    public string? MatriculaPrescriptor {get;set;}
    public string? NombreApellidoPrescriptor {get;set;}
    public required int ValidacionId {get;set;}
    public required string Username {get;set;}
    public required Guid IdFacturador {get;set;}
    public required int TipoIngreso {get;set;}
    public Guid? IdDiagnostico {get;set;}
    public required List<PracticaASolicitar> Prestaciones {get;set;}
}

public class SolicitudPracticaResponse
{
    public Guid Id { get; set; }
    public Guid SolicitudId { get; set; }
    public required string CodigoPractica { get; set; }
    public required string DescripcionPractica { get; set; }
    public required string DescripcionDiagnostico { get; set; }
    public int Cantidad { get; set; }
}

public class SolicitudesResponse
{
    public required Guid Id { get; set; }
    public required string MatriculaEfector { get; set; }
    public required string NombreApellidoEfector { get; set; }
    public string? MatriculaPrescriptor { get; set; }
    public string? NombreApellidoPrescriptor { get; set; }
    public required Validacion Validacion { get; set; }
    public required string Username { get; set; }
    public required Facturador Facturador { get; set; }
    public required string TipoIngreso { get; set; }
    public required DateTime Fecha { get; set; }
    public string? Diagnostico { get; set; }
    public required List<SolicitudPracticaResponse> Prestaciones { get; set; }
}