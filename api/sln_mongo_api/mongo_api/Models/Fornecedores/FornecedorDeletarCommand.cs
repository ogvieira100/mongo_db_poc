using MediatR;
using mongo_api.Models.Fornecedores;

namespace mongo_api.Models.Fornecedores
{
    public class FornecedorDeletarCommand : IRequest<FornecedorResponse>
    {
        public Guid Id { get; set; }
    }
}
