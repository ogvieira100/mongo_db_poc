using Microsoft.EntityFrameworkCore;
using mongo_api.Data.Context;

namespace mongo_api.Data.Repository
{
    public class BaseRepository<TEntity>
           : IBaseRepository<TEntity> where TEntity : class
    {
        public IUnitOfWork UnitOfWork { get; }

        readonly AplicationContext _aplicationContext;

        public IBaseConsultRepository<TEntity> RepositoryConsult { get; protected set; }

        readonly DbSet<TEntity> DbSet;
        public BaseRepository(IUnitOfWork _unitOfWork,
                                 AplicationContext aplicationContext,
                                 IBaseConsultRepository<TEntity> _repositoryConsult
                                 )
        {
            _aplicationContext = aplicationContext;
            UnitOfWork = _unitOfWork;
            RepositoryConsult = _repositoryConsult;
            DbSet = _aplicationContext.Set<TEntity>();
        }
        public void Add(TEntity entity) => DbSet.Add(entity);

        public void Dispose() => GC.SuppressFinalize(this);

        public void Remove(TEntity entity) => DbSet.Remove(entity);

        public void Remove<T>(T entity) where T : class => _aplicationContext.Set<T>().Remove(entity);

        public void Update(TEntity entity) => DbSet.Update(entity);

        public async Task AddAsync(TEntity entidade) => await DbSet.AddAsync(entidade);

        public async Task AddAsync<T>(T entidade) where T : class
        => await _aplicationContext.Set<T>().AddAsync(entidade);

        public void Update<T>(T entity) where T : class
        => _aplicationContext.Set<T>().Update(entity);

        public void UpdateRange<T>(IEnumerable<T> entity) where T : class
        => _aplicationContext.Set<T>().UpdateRange(entity);
    }

}
