using mongo_api.Data.Repository;
using mongo_api.Models.Cliente;
using mongo_api.Models.Produto;

namespace mongo_api.Models.Pedidos
{
    public interface IPedidoQuery 
    {

        Task<PedidoMongo> GetPedidoByRelationalId(string relationalId);


        Task<PedidoMongo> GetPedidoUpdateByRelationalId(string relationalId);

        Task<PagedDataResponse<PedidoMongo>> PagedPedidos(PedidoPagedRequest clientePagedRequest);
    }
    public class PedidoQuery : IPedidoQuery
    {
        readonly IPedidoMongoRepository _pedidoMongoRepository;

        
        public PedidoQuery(IPedidoMongoRepository pedidoMongoRepository)
        {

            _pedidoMongoRepository = pedidoMongoRepository; 
        }
        public async Task<PedidoMongo> GetPedidoByRelationalId(string relationalId)
        => await _pedidoMongoRepository.GetPedidoByRelationalId(relationalId);

        public async Task<PedidoMongo> GetPedidoUpdateByRelationalId(string relationalId)
        => await _pedidoMongoRepository.GetPedidoUpdateByRelationalId(relationalId);

        public async Task<PagedDataResponse<PedidoMongo>> PagedPedidos(PedidoPagedRequest clientePagedRequest)
         => await _pedidoMongoRepository.PagedPedidos(clientePagedRequest);

    }
}
