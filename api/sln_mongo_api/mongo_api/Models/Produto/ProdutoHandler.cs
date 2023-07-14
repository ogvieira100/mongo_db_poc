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
        readonly IProdutoQuery _produtoQuery;

        public ProdutoHandler(IUnitOfWork unitOfWork, 
            IProdutoQuery produtoQuery,
            IBaseRepository<Produtos> produtoRepository)
        {
            _unitOfWork = unitOfWork;
            _produtoQuery = produtoQuery;   
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
            var produtoMongo =  await _produtoQuery.GetProdutoMongoByRelationId(request.Id.ToString());
            var produto = new Produtos();

            produto.Id = request.Id;
            produto.Descricao = request.Descricao;

            _produtoRepository.Update(produto);
            await _unitOfWork.CommitAsync();    

            return novoProduto;
        }

        public Task<ProdutoResponse> Handle(ProdutoDeletarCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
