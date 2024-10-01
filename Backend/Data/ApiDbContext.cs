using Microsoft.EntityFrameworkCore;
using api.Model;

namespace api.Data;

public class ApiDbContext : DbContext
{
    public ApiDbContext(DbContextOptions<ApiDbContext> options): base(options) { }
    public DbSet<Partido> Partidos {get;set;}
    public DbSet<Localidad> Localidades {get;set;}
    public DbSet<Facturador> Facturadores {get;set;}
    public DbSet<Usuario> Usuarios {get;set;}
    public DbSet<Nivel> Niveles {get;set;}
    public DbSet<UsuarioNivelFacturador> UsuariosNivelFacturador {get;set;}
    public DbSet<Profesional> Profesionales {get;set;}
    public DbSet<ProfesionalFacturador> ProfesionalesFacturador {get;set;}
    public DbSet<Practica> Practicas {get;set;}
    public DbSet<AuthIOMA> AuthIOMA {get;set;}
    public DbSet<Paciente> Pacientes {get;set;}
    public DbSet<Solicitud> Solicitudes {get;set;}
    public DbSet<SolicitudPractica> SolicitudPracticas {get;set;}
    public DbSet<Validacion> Validaciones {get;set;}
    public DbSet<ObraSocial> ObrasSociales {get;set;}
    public DbSet<Afiliacion> Afiliaciones {get;set;}
    public DbSet<Especialidad> Especialidades {get;set;}
    public DbSet<ProfesionalEspecialidad> ProfesionalEspecialidades {get;set;}
    public DbSet<Diagnostico> Diagnosticos {get;set;}
}
