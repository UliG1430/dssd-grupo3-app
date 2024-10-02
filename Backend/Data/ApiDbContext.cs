using Backend.Model;
using Microsoft.EntityFrameworkCore;

namespace Backend.Data;

public class ApiDbContext : DbContext
{
    public ApiDbContext(DbContextOptions<ApiDbContext> options): base(options) { }

    public DbSet<Orden> Ordenes { get; set; }
}
