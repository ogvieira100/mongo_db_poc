using MediatR;

namespace mongo_api.Models.Produto
{
    public class ProdutoAtualizarCommand : IRequest<ProdutoResponse>
    {

        public Guid Id { get; set; }
        public string Descricao { get; set; } = ""; 

    }
}
