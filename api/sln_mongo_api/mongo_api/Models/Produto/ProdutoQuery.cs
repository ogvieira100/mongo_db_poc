using mongo_api.Data.Repository;
using mongo_api.Models.Cliente;

namespace mongo_api.Models.Produto
{

    public interface IProdutoQuery
    {

        Task<ProdutoMongo> GetProdutoMongoByRelationId(string relationalId);

        Task<IEnumerable<ProdutoMongo>> GetProdutosMongoByRelationsIds(IEnumerable<string> relationalsIds);

        Task<PagedDataResponse<ProdutoMongo>> PagedProdutos(ProdutoPagedRequest clientePagedRequest);

    }
    public class ProdutoQuery: IProdutoQuery
    {

        readonly IProdutoMongoRepository _produtoMongoRepository;

        public ProdutoQuery(IProdutoMongoRepository produtoMongoRepository)
        {
            _produtoMongoRepository = produtoMongoRepository;
        }

        public async Task<ProdutoMongo> GetProdutoMongoByRelationId(string relationalId)
        => await _produtoMongoRepository.BaseConsultRepositoryMongo.GetByIdAsync(relationalId);

        public async Task<IEnumerable<ProdutoMongo>> GetProdutosMongoByRelationsIds(IEnumerable<string> relationalsIds)
        => await _produtoMongoRepository.BaseConsultRepositoryMongo.SearchAsync(x => relationalsIds.Contains(x.RelationalId));
        

        public async Task<PagedDataResponse<ProdutoMongo>> PagedProdutos(ProdutoPagedRequest clientePagedRequest)
        => await _produtoMongoRepository.BaseConsultRepositoryMongo.PaginateAsync(clientePagedRequest, null);
    }
}
