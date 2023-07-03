using mongo_api.Models;
using MongoDB.Driver;

namespace mongo_api.Data.Repository
{
    public interface IBaseRepositoryMongo<TEntity>
        : IDisposable where TEntity : BaseMongo
    {
        void Add(TEntity entidade);

        void AddMany(IEnumerable<TEntity> entidade);

        Task AddAsync(TEntity entidade);

        Task AddManyAsync(IEnumerable<TEntity> entidade);
        void Update(TEntity customer);
        Task UpdateAsync(TEntity customer);
        void Remove(TEntity customer);
        Task RemoveAsync(TEntity customer);
        void RemoveMany(IEnumerable<TEntity> customer);

        IMongoCollection<TEntity> MongoCollectionPersist { get; }

        IBaseConsultRepositoryMongo<TEntity> BaseConsultRepositoryMongo { get; }

    }
}
