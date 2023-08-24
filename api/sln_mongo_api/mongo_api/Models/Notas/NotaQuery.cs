using mongo_api.Data.Repository;
using mongo_api.Models.Pedidos;
using mongo_api.Models.Produto;

namespace mongo_api.Models.Notas
{
    public interface INotaQuery
    {
        Task<NotaMongo> GetNotaByRelationalId(string relationalId);
        Task<NotaMongo> GetNotaUpdateByRelationalId(string relationalId);
        Task<PagedDataResponse<NotaMongo>> PagedNotas(NotaPagedRequest clientePagedRequest);

    }
    public class NotaQuery : INotaQuery
    {

        readonly INotaMongoRepository _notaMongoRepository;


        public NotaQuery(INotaMongoRepository notaMongoRepository)
        {
            _notaMongoRepository = notaMongoRepository; 
        }

        /*
         
         public async Task<PedidoMongo> GetPedidoByRelationalId(string relationalId)
        => await _pedidoMongoRepository.GetPedidoByRelationalId(relationalId);

        public async Task<PedidoMongo> GetPedidoUpdateByRelationalId(string relationalId)
        => await _pedidoMongoRepository.GetPedidoUpdateByRelationalId(relationalId);
         
         */

        public Task<NotaMongo> GetNotaByRelationalId(string relationalId)
        {
            throw new NotImplementedException();
        }

        public async Task<NotaMongo> GetNotaUpdateByRelationalId(string relationalId)
        => await _notaMongoRepository.GetNotaUpdateByRelationalId(relationalId);

        public async Task<PagedDataResponse<NotaMongo>> PagedNotas(NotaPagedRequest clientePagedRequest)
         => await _notaMongoRepository.BaseConsultRepositoryMongo.PaginateAsync(clientePagedRequest, null);
    }
}
