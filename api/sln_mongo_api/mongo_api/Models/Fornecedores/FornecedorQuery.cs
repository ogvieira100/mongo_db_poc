using mongo_api.Data.Repository;
using mongo_api.Models.Produto;

namespace mongo_api.Models.Fornecedores
{
    public interface IFornecedorQuery
    {
        Task<FornecedorMongo> GetFornecedorMongoByRelationId(string relationalId);

        Task<PagedDataResponse<FornecedorMongo>> PagedFornecedores(FornecedorPagedRequest  fornecedorPagedRequest);
    }
    public class FornecedorQuery: IFornecedorQuery
    {

        readonly IFornecedorMongoRepository _fornecedorMongoRepository;

        public FornecedorQuery(IFornecedorMongoRepository fornecedorMongoRepository)
        {
            _fornecedorMongoRepository = fornecedorMongoRepository;
        }

        public async Task<FornecedorMongo> GetFornecedorMongoByRelationId(string relationalId)
        => await _fornecedorMongoRepository.BaseConsultRepositoryMongo.GetByIdAsync(relationalId);

        public async Task<PagedDataResponse<FornecedorMongo>> PagedFornecedores(FornecedorPagedRequest clientePagedRequest)
        => await _fornecedorMongoRepository.BaseConsultRepositoryMongo.PaginateAsync(clientePagedRequest, null);
    }
}
