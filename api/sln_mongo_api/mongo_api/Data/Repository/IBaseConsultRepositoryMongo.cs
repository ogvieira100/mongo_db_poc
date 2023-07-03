using mongo_api.Models;
using MongoDB.Driver;
using System.Linq.Expressions;

namespace mongo_api.Data.Repository
{
    public interface IBaseConsultRepositoryMongo<TEntity> : IDisposable where TEntity : BaseMongo
    {
        Task<IEnumerable<TEntity>> SearchAsync(Expression<Func<TEntity, bool>> predicate);

        Task<IEnumerable<TEntity>> SearchTextAsync(string text);
        
        Task<TEntity> GetByIdAsync(string id);
        IMongoCollection<TEntity> MongoCollectionConsult { get; }
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate);

        Task<PagedDataResponse<TEntity>> PaginateAsync(PagedDataRequest pagedDataRequest, Expression<Func<TEntity, bool>>? predicate);
    }
}
