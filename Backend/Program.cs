using Microsoft.EntityFrameworkCore;
using api.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using ApiACEAPP.Repositories;
using ApiACEAPP.Services;
using System.Diagnostics.Tracing;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ApiDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();

//Repositories
builder.Services.AddScoped<UsuarioRepository>();
builder.Services.AddScoped<NivelRepository>();
builder.Services.AddScoped<FacturadorRepository>();
builder.Services.AddScoped<UsuarioNivelFacturadorRepository>();
builder.Services.AddScoped<AuthIOMARepository>();
builder.Services.AddScoped<PracticaRepository>();
builder.Services.AddScoped<PacienteRepository>();
builder.Services.AddScoped<SolicitudRepository>();
builder.Services.AddScoped<ValidacionRepository>();
builder.Services.AddScoped<ObraSocialRepository>();
builder.Services.AddScoped<AfiliacionRepository>();
builder.Services.AddScoped<ProfesionalRepository>();
builder.Services.AddScoped<ProfesionalFacturadorRepository>();
builder.Services.AddScoped<EspecialidadRepository>();
builder.Services.AddScoped<DiagnosticoRepository>();

//Servicio IOMA
builder.Services.AddScoped<IOMAService>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
                      policy =>
                      {
                          policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
                      });
});

builder.Services.AddAuthorization();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateIssuerSigningKey = true,
            ValidateLifetime = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["ConfigurationJwt:SecretKey"]!)
            )
        };
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

//app.UseHttpsRedirection();

app.UseCors("AllowSpecificOrigin");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
