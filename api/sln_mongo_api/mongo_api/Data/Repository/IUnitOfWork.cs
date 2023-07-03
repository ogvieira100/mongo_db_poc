namespace mongo_api.Data.Repository
{
    public interface IUnitOfWork:IDisposable
    {
        Task<bool> CommitAsync();
    }
}
