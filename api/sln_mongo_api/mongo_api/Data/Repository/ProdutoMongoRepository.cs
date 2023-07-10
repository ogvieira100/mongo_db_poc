using mongo_api.Data.Context;
using mongo_api.Models.Cliente;
using mongo_api.Models.Produto;

namespace mongo_api.Data.Repository
{
    public interface IProdutoMongoRepository : IBaseRepositoryMongo<ProdutoMongo>
    {

    }

    public class ProdutoMongoRepository : BaseRepositoryMongo<ProdutoMongo>,
        IProdutoMongoRepository
    {
        public ProdutoMongoRepository(MongoContext mongoContext,
            IBaseConsultRepositoryMongo<ProdutoMongo> baseConsultRepositoryMongo) 
            : base(mongoContext, baseConsultRepositoryMongo)
        {


        }
    }
}
