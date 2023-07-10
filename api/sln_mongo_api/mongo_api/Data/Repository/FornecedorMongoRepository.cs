using mongo_api.Data.Context;
using mongo_api.Models.Cliente;
using mongo_api.Models.Fornecedores;

namespace mongo_api.Data.Repository
{
    public interface IFornecedorMongoRepository : IBaseRepositoryMongo<FornecedorMongo>
    { 
    
    }
    public class FornecedorMongoRepository : BaseRepositoryMongo<FornecedorMongo>, IFornecedorMongoRepository
    {
        public FornecedorMongoRepository(MongoContext mongoContext, IBaseConsultRepositoryMongo<FornecedorMongo> baseConsultRepositoryMongo) : base(mongoContext, baseConsultRepositoryMongo)
        {

        }
    }
}
