using System.Linq;
using System.Threading.Tasks;
using Backend.Model;
using Backend.Data;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repositories
{
    public class NotificacionPagoRepository : GenericRepository<NotificacionPago>
    {
        private readonly ApiDbContext _context;

        public NotificacionPagoRepository(ApiDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<NotificacionPago> GetByCaseIdAsync(int caseId)
        {
            return await _context.Set<NotificacionPago>()
                .FirstOrDefaultAsync(np => np.caseId == caseId);
        }

        public async Task AddNotificacionPagoAsync(NotificacionPago notificacionPago)
        {
            await _context.Set<NotificacionPago>().AddAsync(notificacionPago);
            await _context.SaveChangesAsync();
        }
    }
}
