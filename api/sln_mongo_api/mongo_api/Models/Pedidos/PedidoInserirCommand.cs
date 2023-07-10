using MediatR;
using mongo_api.Models.Cliente;
using mongo_api.Models.Fornecedores;
using mongo_api.Models.Produto;

namespace mongo_api.Models.Pedidos
{
    public class PedidoInserirCommand : IRequest<PedidoResponse>
    {
        public virtual Guid ClienteId { get; set; }
        public virtual Guid FornecedorId { get; set; }
        public string Observation { get; set; }
        public IEnumerable<PedidoItensDto> PedidoItensDto { get; set; }

        public PedidoInserirCommand()
        {
            PedidoItensDto = new List<PedidoItensDto>();    
        }

    }
}
