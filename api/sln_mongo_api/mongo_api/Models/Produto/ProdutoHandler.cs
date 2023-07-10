using MediatR;
using mongo_api.Data.Repository;
using mongo_api.Models.Cliente;

namespace mongo_api.Models.Produto
{
    public class ProdutoHandler 
        : IRequestHandler<ProdutoInserirCommand, ProdutoResponse>,
          IRequestHandler<ProdutoAtualizarCommand, ProdutoResponse>,
          IRequestHandler<ProdutoDeletarCommand, ProdutoResponse>
    {

        readonly IUnitOfWork _unitOfWork;
        readonly IBaseRepository<Produtos> _produtoRepository;

        public ProdutoHandler(IUnitOfWork unitOfWork, IBaseRepository<Produtos> produtoRepository)
        {
            _unitOfWork = unitOfWork;   
            _produtoRepository = produtoRepository; 
        }
        public async Task<ProdutoResponse> Handle(ProdutoInserirCommand request, CancellationToken cancellationToken)
        {
            var resp = new ProdutoResponse();

            var novoProduto = new Produtos();
            novoProduto.Descricao = request.Descricao;

            await _produtoRepository.AddAsync(novoProduto);
            await _unitOfWork.CommitAsync();
            return resp;
        }

        public async Task<ProdutoResponse> Handle(ProdutoAtualizarCommand request, CancellationToken cancellationToken)
        {
            var novoProduto = new ProdutoResponse();
            

            return novoProduto;
        }

        public Task<ProdutoResponse> Handle(ProdutoDeletarCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
