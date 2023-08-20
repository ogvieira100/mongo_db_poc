using MediatR;

namespace mongo_api.Models.Pedidos
{
    public class PedidoIncluidoEvent: Event
    {
        public Guid PedidoId { get; set; }

    }
}
