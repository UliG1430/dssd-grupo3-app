using Backend.Data;
using Backend.Model;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Backend.Repositories
{
    public class UsuarioRepository : GenericRepository<Usuario>
    {
        private readonly ApiDbContext _dbContext;

        public UsuarioRepository(ApiDbContext context) : base(context)
        {
            _dbContext = context; // Initialize the DbContext
        }

        public async Task<Usuario> GetUsuarioByUsernameAsync(string username)
        {
            return await _dbContext.Usuarios
                .FirstOrDefaultAsync(u => u.UsuarioNombre == username);
        }

        public async Task<Usuario> GetByIdAsync(int id)
        {
            return await _dbContext.Usuarios.FindAsync(id);
        }

        public void Update(Usuario usuario)
        {
            _dbContext.Usuarios.Update(usuario);
        }

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

    }


}
