using api.Data;
using api.Model;

namespace ApiACEAPP.Repositories
{
    public class AuthIOMARepository : GenericRepository<AuthIOMA>
    {
        public AuthIOMARepository(ApiDbContext context) : base(context)
        {
        }
    }
}