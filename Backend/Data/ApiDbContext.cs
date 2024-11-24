using Microsoft.EntityFrameworkCore;
using Backend.Model;

namespace Backend.Data
{
    public class ApiDbContext : DbContext
    {
        public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options) { }

        public DbSet<Orden> Ordenes { get; set; }
        public DbSet<PuntoRecoleccion> PuntosRecoleccion { get; set; }
        public DbSet<Material> Materiales { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Orden Table
            modelBuilder.Entity<Orden>(entity =>
            {
                entity.HasKey(o => o.Id);
                entity.Property(o => o.Id)
                      .ValueGeneratedOnAdd(); // Auto-increment
                entity.Property(o => o.Material)
                      .IsRequired();
                entity.Property(o => o.PesoKg)
                      .IsRequired();
                entity.Property(o => o.PuntoRecoleccion)
                      .IsRequired();
                entity.Property(o => o.Fecha)
                      .IsRequired();
                entity.Property(o => o.CaseId)
                      .IsRequired();
                entity.Property(o => o.UsuarioId)
                        .IsRequired();
            });

            // PuntoRecoleccion Table
            modelBuilder.Entity<PuntoRecoleccion>(entity =>
            {
                entity.HasKey(pr => pr.Id);
                entity.Property(pr => pr.Id)
                      .ValueGeneratedOnAdd(); // Auto-increment
                entity.Property(pr => pr.Nombre)
                      .IsRequired();
                entity.Property(pr => pr.Ubicacion)
                      .IsRequired();
            });

            // Material Table
            modelBuilder.Entity<Material>(entity =>
            {
                entity.HasKey(m => m.Id);
                entity.Property(m => m.Id)
                      .ValueGeneratedOnAdd(); // Auto-increment
                entity.Property(m => m.Nombre)
                      .IsRequired();
            });

            // Usuario Table
            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.HasKey(u => u.Id);
                entity.Property(u => u.Id)
                      .ValueGeneratedOnAdd(); // Auto-increment
                entity.Property(u => u.UsuarioNombre)
                      .IsRequired();
                entity.Property(u => u.Password)
                      .IsRequired();
                entity.Property(u => u.comenzoRecorrido)
                        .IsRequired();
                entity.Property(u => u.caseId)
                        .IsRequired();
            });
        }
    }
}
