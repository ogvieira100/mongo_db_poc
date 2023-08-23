using MediatR;
using mongo_api.Models.Pedidos;

namespace mongo_api.Models.Notas
{
    public class NotaInserirCommand : IRequest<NotaResponse>
    {

        public virtual Guid ClienteId { get; set; }
        public virtual Guid FornecedorId { get; set; }
        public string Observation { get; set; }
        public string Numero { get; set; }
        public IEnumerable<NotaItensDto> NotaItens { get; set; }

        public NotaInserirCommand()
        {
            NotaItens = new List<NotaItensDto>();
        }

    }
}
