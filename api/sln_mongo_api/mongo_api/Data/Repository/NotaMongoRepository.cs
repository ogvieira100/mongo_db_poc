using mongo_api.Data.Context;
using mongo_api.Models;
using mongo_api.Models.Notas;
using mongo_api.Models.Pedidos;
using MongoDB.Driver;

namespace mongo_api.Data.Repository
{
    public interface INotaMongoRepository : IBaseRepositoryMongo<NotaMongo>
    {
        Task<NotaMongo> GetNotaUpdateByRelationalId(string relationalId);
    }
    public class NotaMongoRepository : BaseRepositoryMongo<NotaMongo>, INotaMongoRepository
    {
        public NotaMongoRepository(MongoContext mongoContext,
            IBaseConsultRepositoryMongo<NotaMongo> baseConsultRepositoryMongo) 
            : base(mongoContext, baseConsultRepositoryMongo)
        {


        }

        
        public async Task<NotaMongo> GetNotaUpdateByRelationalId(string relationalId)
        => await (MongoCollectionPersist.Find(x => x.RelationalId == relationalId)).FirstOrDefaultAsync();

    }
}
