using MediatR;

namespace mongo_api.Models.Cliente
{
    public class ClienteAtualizarCommand : IRequest<ClienteResponse>
    {

        public Guid Id { get; set; }
        public string CPF { get; set; } = "";
        public string? Nome { get; set; } = "";

        public IEnumerable<EnderecoAtualizarDto> Enderecos { get; set; }

        public ClienteAtualizarCommand()
        {
            Enderecos = new List<EnderecoAtualizarDto>();
        }

    }
}
