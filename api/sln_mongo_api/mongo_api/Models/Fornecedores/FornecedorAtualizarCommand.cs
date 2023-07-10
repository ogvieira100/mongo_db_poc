using MediatR;
using mongo_api.Models.Fornecedores;

namespace mongo_api.Models.Fornecedores
{
    public class FornecedorAtualizarCommand : IRequest<FornecedorResponse>
    {

        public Guid Id { get; set; }
        public string CNPJ { get; set; }
        public string RazaoSocial { get; set; }
    }
}
