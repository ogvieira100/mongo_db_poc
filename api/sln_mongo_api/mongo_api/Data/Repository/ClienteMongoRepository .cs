using mongo_api.Data.Context;
using mongo_api.Models.Cliente;

namespace mongo_api.Data.Repository
{

    public interface IClienteMongoRepository 
        : IBaseRepositoryMongo<ClientesMongo>
    { 
    
    
    }

    public class ClienteMongoRepository : BaseRepositoryMongo<ClientesMongo>,
        IClienteMongoRepository
    {
        public ClienteMongoRepository(MongoContext mongoContext,
            IBaseConsultRepositoryMongo<ClientesMongo> _baseConsultRepositoryMongo) : base(mongoContext, _baseConsultRepositoryMongo)
        {

              
        }
    }
}
