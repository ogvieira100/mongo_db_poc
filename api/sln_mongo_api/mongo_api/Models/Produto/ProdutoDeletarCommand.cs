using MediatR;

namespace mongo_api.Models.Produto
{
    public class ProdutoDeletarCommand : IRequest<ProdutoResponse>
    {
        public Guid Id { get; set; }
    }
}
