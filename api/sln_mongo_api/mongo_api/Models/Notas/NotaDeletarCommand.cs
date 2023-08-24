using MediatR;
using mongo_api.Models.Pedidos;

namespace mongo_api.Models.Notas
{
    public class NotaDeletarCommand : IRequest<NotaResponse>
    {
        public Guid Id { get; set; }
    }
}
