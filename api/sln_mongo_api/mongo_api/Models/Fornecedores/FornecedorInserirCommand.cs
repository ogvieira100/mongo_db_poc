using MediatR;
using mongo_api.Models.Produto;

namespace mongo_api.Models.Fornecedores
{
    public class FornecedorInserirCommand : IRequest<FornecedorResponse>
    {

        public string CNPJ { get; set; }

        public string RazaoSocial { get; set; }
    }
}
