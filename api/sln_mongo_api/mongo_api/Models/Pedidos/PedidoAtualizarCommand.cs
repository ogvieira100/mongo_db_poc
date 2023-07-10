using MediatR;

namespace mongo_api.Models.Pedidos
{
    public class PedidoAtualizarCommand : IRequest<PedidoResponse>
    {

        public Guid Id { get; set; }
        public  Guid ClienteId { get; set; }
        public  Guid FornecedorId { get; set; }
        public string Observation { get; set; }
        public IEnumerable<PedidoItensDto> PedidoItensDto { get; set; }

        public PedidoAtualizarCommand()
        {
            PedidoItensDto = new List<PedidoItensDto>();
        }
    }
}
