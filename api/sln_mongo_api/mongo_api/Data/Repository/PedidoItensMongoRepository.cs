using mongo_api.Models.Pedidos;
using mongo_api.Models;
using mongo_api.Data.Context;

namespace mongo_api.Data.Repository
{

    public interface IPedidoItensMongoRepository : IBaseRepositoryMongo<PedidoItensMongo>
    {
        
    }
    public class PedidoItensMongoRepository : BaseRepositoryMongo<PedidoItensMongo>, IPedidoItensMongoRepository
    {
        public PedidoItensMongoRepository(MongoContext mongoContext,
            IBaseConsultRepositoryMongo<PedidoItensMongo> baseConsultRepositoryMongo)
            : base(mongoContext, baseConsultRepositoryMongo)
        {



        }
    }
}
