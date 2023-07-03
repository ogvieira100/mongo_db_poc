namespace mongo_api.Data.Repository
{
    public interface IBaseRepository<TEntity> : IDisposable where TEntity : class
    {
        void Add(TEntity entidade);

        Task AddAsync(TEntity entidade);

        Task AddAsync<T>(T entidade) where T : class;

        void Update(TEntity customer);

        void Update<T>(T entity) where T : class;

        void UpdateRange<T>(IEnumerable<T> entity) where T : class;

        void Remove(TEntity customer);

        void Remove<T>(T customer) where T : class;

        IUnitOfWork UnitOfWork { get; }

        IBaseConsultRepository<TEntity> RepositoryConsult { get; }

    }
}
