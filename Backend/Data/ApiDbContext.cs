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
        public DbSet<Paquete> Paquetes { get; set; }
        public DbSet<UltimaEvaluacion> UltimasEvaluaciones { get; set; }

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
                entity.Property(o => o.PuntoRecoleccionId)
                      .IsRequired();
                entity.Property(o => o.Fecha)
                      .IsRequired();
                entity.Property(o => o.CaseId)
                      .IsRequired();
                entity.Property(o => o.UsuarioId)
                        .IsRequired();
                entity.Property(o => o.paqueteId)
                        .IsRequired();
                entity.Property(o => o.revisado)
                        .IsRequired();
                entity.Property(o => o.estado)
                        .IsRequired();

                entity.HasOne(o => o.PuntoRecoleccion)
                              .WithMany()
                              .HasForeignKey(o => o.PuntoRecoleccionId);

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
                entity.Property(m => m.StockActual)
                      .IsRequired()
                      .HasDefaultValue(0); // Stock por defecto 0
                entity.HasData(
                    new Material { Id = 1, Nombre = "Madera", StockActual = 0 },
                    new Material { Id = 2, Nombre = "Cartón", StockActual = 0 },
                    new Material { Id = 3, Nombre = "Plástico", StockActual = 0 },
                    new Material { Id = 4, Nombre = "Vidrio", StockActual = 0 },  // Otro material sugerido
                    new Material { Id = 5, Nombre = "Metal", StockActual = 0 }    // Otro material sugerido
                );
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
                entity.Property(u => u.rol)
                      .IsRequired();
                entity.Property(u => u.seleccionoPaquete)
                      .IsRequired();
                entity.Property(u => u.paqueteId)
                      .IsRequired();

                // Insertar los dos usuarios con caseId = 0
                entity.HasData(
                    new Usuario
                    {
                        Id = 1,
                        UsuarioNombre = "walter.bates",
                        Password = "bpm",
                        comenzoRecorrido = false,
                        caseId = 0,
                        rol = "R", // Asignamos el rol "R"
                        seleccionoPaquete = false,
                        paqueteId = 0
                    },
                    new Usuario
                    {
                        Id = 2,
                        UsuarioNombre = "william.jobs",
                        Password = "bpm",
                        comenzoRecorrido = false,
                        caseId = 0,
                        rol = "A", // Asignamos el rol "A"
                        seleccionoPaquete = false,
                        paqueteId = 0
                    }
                );
            });


            // Configure Paquete
            modelBuilder.Entity<Paquete>(entity =>
            {
                entity.HasKey(p => p.Id);
                entity.Property(p => p.Id)
                      .ValueGeneratedOnAdd(); // Auto-increment
                entity.Property(p => p.CaseId)
                      .IsRequired(); // CaseId is required
                entity.Property(p => p.State)
                        .IsRequired(); // State is required
            });

            // Configure UltimaEvaluacion
            modelBuilder.Entity<UltimaEvaluacion>(entity =>
            {
                entity.HasKey(ue => ue.Id);
                entity.Property(ue => ue.Id)
                      .ValueGeneratedOnAdd(); // Auto-increment
                entity.Property(ue => ue.Fecha)
                      .IsRequired(); // Fecha is required
            });

            modelBuilder.Entity<NotificacionPago>(entity =>
            {
                entity.HasKey(np => np.Id);
                entity.Property(np => np.Id)
                      .ValueGeneratedOnAdd(); // Auto-increment
                entity.Property(np => np.cantidad)
                      .IsRequired(); // cantidad is required
                entity.Property(np => np.caseId)
                      .IsRequired(); // caseId is required
            });

            modelBuilder.Entity<Evaluacion>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id)
                      .ValueGeneratedOnAdd(); // Auto-increment
                entity.Property(e => e.caseId)
                      .IsRequired(); // CaseId is required
                entity.Property(e => e.state)
                      .IsRequired(); // Fecha is required
                entity.Property(e => e.observaciones)
                      .IsRequired(); // Puntaje is required
                entity.Property(e => e.cantOrdenes)
                        .IsRequired(); // Puntaje is required
                entity.Property(e => e.cantOrdenesOk)
                        .IsRequired(); // Puntaje is required
                entity.Property(e => e.cantOrdenesMal)
                        .IsRequired(); // Puntaje is required
            });
        }
    }
}
