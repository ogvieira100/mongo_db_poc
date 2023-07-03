using System.Linq.Expressions;

namespace mongo_api.Data.Repository
{
    public interface IBaseConsultRepository<TEntity> : IDisposable where TEntity : class
    {
        Task<IEnumerable<TEntity>> SearchAsync(Expression<Func<TEntity, bool>> predicate);
        IQueryable<TEntity> GetQueryable();
        Task<TEntity> GetByIdAsync(Int64 id);
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate);
        Task<IEnumerable<TModel>> GetAllAsync<TModel>(string sql, object obj = null);
    }
}
