using mongo_api.Data.Context;
using mongo_api.Models;
using mongo_api.Models.Notas;
using mongo_api.Models.Pedidos;

namespace mongo_api.Data.Repository
{
    public interface INotaMongoRepository : IBaseRepositoryMongo<NotaMongo>
    {
     
    }
    public class NotaMongoRepository : BaseRepositoryMongo<NotaMongo>, INotaMongoRepository
    {
        public NotaMongoRepository(MongoContext mongoContext,
            IBaseConsultRepositoryMongo<NotaMongo> baseConsultRepositoryMongo) 
            : base(mongoContext, baseConsultRepositoryMongo)
        {


        }

    
    }
}
