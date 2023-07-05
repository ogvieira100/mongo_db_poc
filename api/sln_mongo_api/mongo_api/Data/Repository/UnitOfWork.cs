using Microsoft.EntityFrameworkCore;
using mongo_api.Data.Context;
using mongo_api.Models.Cliente;
using MongoDB.Driver;

namespace mongo_api.Data.Repository
{

    public class UnitOfWork : IUnitOfWork
    {

        readonly AplicationContext _aplicationContext;
        readonly MongoContext _mongoContext;
        IMongoCollection<ClientesMongo> _clientesCollection;
        IMongoCollection<EnderecoMongo> _enderecosCollection;
        public UnitOfWork(AplicationContext aplicationContext, MongoContext mongoContext)
        {
            _aplicationContext = aplicationContext;
            _mongoContext = mongoContext;
            _clientesCollection = _mongoContext.DB.GetCollection<ClientesMongo>("clientes");
            _enderecosCollection = _mongoContext.DB.GetCollection<EnderecoMongo>("endereco");
        }


        public async Task<bool> CommitAsync()
        => await _aplicationContext.SaveChangesAsync() > 0;
      

        public void Dispose() => GC.SuppressFinalize(this);
    }
}
