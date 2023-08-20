using MediatR;

namespace mongo_api.Models.Pedidos
{
    public class PedidoEventHandler : INotificationHandler<PedidoIncluidoEvent>
    {
        public Task Handle(PedidoIncluidoEvent notification, CancellationToken cancellationToken)
        {
            /*enviar email notificando que houve um pedido*/
            // Enviar evento de confirmação
            return Task.CompletedTask;
        }
    }
}
