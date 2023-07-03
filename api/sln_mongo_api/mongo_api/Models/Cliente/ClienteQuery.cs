using mongo_api.Data.Repository;

namespace mongo_api.Models.Cliente
{
    public interface IClienteQuery
    {
        Task<ClientesMongo> GetCliMongoByRelationId(string relationalId);

        Task<PagedDataResponse<ClientesMongo>> PagedCliente(ClientePagedRequest clientePagedRequest);
    }
    public class ClienteQuery: IClienteQuery
    {

        readonly IClienteMongoRepository _clienteMongoRepository;

        public ClienteQuery(IClienteMongoRepository clienteMongoRepository)
        {
            _clienteMongoRepository = clienteMongoRepository;    
        }

        public async Task<ClientesMongo> GetCliMongoByRelationId(string relationalId)
        => await _clienteMongoRepository.BaseConsultRepositoryMongo.GetByIdAsync(relationalId);

        public async Task<PagedDataResponse<ClientesMongo>> PagedCliente(ClientePagedRequest clientePagedRequest)
        {
            return await _clienteMongoRepository.BaseConsultRepositoryMongo.PaginateAsync(clientePagedRequest, null);
        }
    }
}
