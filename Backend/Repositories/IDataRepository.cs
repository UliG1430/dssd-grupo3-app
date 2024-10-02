using System.Linq.Expressions;

namespace Backend.Repositories
{
    public interface IDataRepository<TEntity>
    {
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<TEntity?> GetAsync(Guid id);
        Task<TEntity> AddAsync(TEntity entity);
        Task<TEntity> UpdateAsync(TEntity dbEntity, TEntity entity);
        void DeleteAsync(Guid id);
        Task<IEnumerable<TEntity>> FilterAsync(
            Expression<Func<TEntity, bool>>? filtro = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
            string includes = "");
    }
}