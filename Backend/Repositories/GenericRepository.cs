using System.Linq.Expressions;
using Backend.Data;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repositories
{
    public class GenericRepository<TEntity> : IDataRepository<TEntity> where TEntity : class
    {
        protected readonly ApiDbContext _context;
        public GenericRepository(ApiDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await _context.Set<TEntity>().ToListAsync();
        }
        public async Task<TEntity?> GetAsync(Guid id)
        {
            return await _context.Set<TEntity>().FindAsync(id);
        }
        public async Task<TEntity> AddAsync(TEntity entity)
        {
            await _context.Set<TEntity>().AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
        public async Task<TEntity> UpdateAsync(TEntity dbEntity, TEntity entity)
        {
            _context.Entry(dbEntity).CurrentValues.SetValues(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
        public async void DeleteAsync(Guid id)
        {
            var entity = await _context.Set<TEntity>().FindAsync(id);
            if (entity != null)
                _context.Set<TEntity>().Remove(entity);
            await _context.SaveChangesAsync();
        }
        public async Task<IEnumerable<TEntity>> FilterAsync(
            Expression<Func<TEntity, bool>>? filtro = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
            string includes = "")
        {
            IQueryable<TEntity> query = _context.Set<TEntity>();
            if (filtro != null)
            {
                query = query.Where(filtro);
            }
            foreach (var include in includes.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(include);
            }
            if (orderBy != null)
            {
                return orderBy(query).ToList();
            }
            return await query.ToListAsync();
        }
    }
}