namespace api.Dto;

public class NivelModel
{
    public required string nombre { get; set; }
}

public class UsuarioNivelFacturadorModel
{
    public required Guid idUsuario { get; set; }
    public required Guid idNivel { get; set; }
    public required Guid idFacturador { get; set; }
}

public class ObraSocialModel
{
    public required string nombre { get; set; }
}