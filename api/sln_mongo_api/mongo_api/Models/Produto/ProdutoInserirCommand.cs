using MediatR;

namespace mongo_api.Models.Produto
{
    public class ProdutoInserirCommand:IRequest<ProdutoResponse>
    {

        public string Descricao { get; set; } = "";

    }
}
