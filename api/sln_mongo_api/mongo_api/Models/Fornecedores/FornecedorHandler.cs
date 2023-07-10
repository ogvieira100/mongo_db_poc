using MediatR;
using mongo_api.Data.Repository;
using mongo_api.Models.Fornecedores;
using mongo_api.Models.Produto;

namespace mongo_api.Models.Fornecedores
{
    public class FornecedorHandler : 
          IRequestHandler<FornecedorInserirCommand, FornecedorResponse>,
          IRequestHandler<FornecedorAtualizarCommand, FornecedorResponse>,
          IRequestHandler<FornecedorDeletarCommand, FornecedorResponse>
    {

        readonly IUnitOfWork _unitOfWork;
        readonly IBaseRepository<Fornecedor>  _fornecedorRepository;

        public FornecedorHandler(IUnitOfWork unitOfWork, IBaseRepository<Fornecedor> fornecedorRepository)
        {
            _unitOfWork = unitOfWork;
            _fornecedorRepository = fornecedorRepository;
        }
        public async Task<FornecedorResponse> Handle(FornecedorInserirCommand request, CancellationToken cancellationToken)
        {
            var resp = new FornecedorResponse();

            var novoFornecedor = new Fornecedor();
            novoFornecedor.CNPJ = request.CNPJ;
            novoFornecedor.RazaoSocial = request.RazaoSocial;

            await _fornecedorRepository.AddAsync(novoFornecedor);
            await _unitOfWork.CommitAsync();
            return resp;
        }

        public Task<FornecedorResponse> Handle(FornecedorAtualizarCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<FornecedorResponse> Handle(FornecedorDeletarCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
