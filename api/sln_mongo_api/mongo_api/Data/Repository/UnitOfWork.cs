using Microsoft.EntityFrameworkCore;
using mongo_api.Data.Context;
using mongo_api.Models.Cliente;
using MongoDB.Driver;

namespace mongo_api.Data.Repository
{

    public class UnitOfWork : IUnitOfWork
    {

        readonly AplicationContext _aplicationContext;

      
        public UnitOfWork(AplicationContext aplicationContext)
        {
            _aplicationContext = aplicationContext;
           
        }


        public async Task<bool> CommitAsync()
        => await _aplicationContext.SaveChangesAsync() > 0;
      

        public void Dispose() => GC.SuppressFinalize(this);
    }
}
