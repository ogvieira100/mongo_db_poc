using MediatR;

namespace mongo_api.Models.Cliente
{
    public class ClienteDeletarCommand : IRequest<ClienteResponse>
    {

        public Guid Id { get; set; }
    }
}
