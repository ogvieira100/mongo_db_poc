using MediatR;
using mongo_api.Data.Repository;
using mongo_api.Models.Cliente;
using mongo_api.Models.Fornecedores;
using mongo_api.Models.Pedidos;
using mongo_api.Models.Produto;

namespace mongo_api.Models.Notas
{
    public class NotaHandler :
        IRequestHandler<NotaInserirCommand, NotaResponse>,
        IRequestHandler<NotaDeletarCommand, NotaResponse>
    {
        readonly IUnitOfWork _unitOfWork;
        readonly IClienteQuery _clienteQuery;
        readonly IPedidoQuery _pedidoQuery;
        readonly IProdutoQuery _produtoQuery;
        readonly IFornecedorQuery _fornedorQuery;
        readonly IBaseRepository<Fornecedor> _fornecedorRepository;
        readonly IBaseRepository<Clientes> _clienteRepository;
        readonly IBaseRepository<Pedido> _pedidoRepository;
        readonly IBaseRepository<PedidoItens> _pedidoItensRepository;
        readonly IPedidoMongoRepository _pedidoMongoRepository;
        readonly IPedidoItensMongoRepository _pedidoItensMongoRepository;

        public NotaHandler(IFornecedorQuery fornedorQuery,
                             IUnitOfWork unitOfWork,
                             IPedidoQuery pedidoQuery,
                             IProdutoQuery produtoQuery,
                             IPedidoMongoRepository pedidoMongoRepository,

                             IBaseRepository<Pedido> pedidoRepository,
                             IBaseRepository<PedidoItens> pedidoItensRepository,
                             IBaseRepository<Clientes> clienteRepository,
                             IBaseRepository<Fornecedor> fornecedorRepository,
                             IClienteQuery clienteQuery,
                             IPedidoItensMongoRepository pedidoItensMongoRepository)
        {
            _unitOfWork = unitOfWork;
            _clienteQuery = clienteQuery;
            _fornedorQuery = fornedorQuery;
            _produtoQuery = produtoQuery;
            _pedidoQuery = pedidoQuery;
            _pedidoRepository = pedidoRepository;
            _pedidoItensRepository = pedidoItensRepository;
            _clienteRepository = clienteRepository;
            _fornecedorRepository = fornecedorRepository;
            _pedidoMongoRepository = pedidoMongoRepository;
            _pedidoItensMongoRepository = pedidoItensMongoRepository;

        }



        public Task<NotaResponse> Handle(NotaDeletarCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<NotaResponse> Handle(NotaInserirCommand request, CancellationToken cancellationToken)
        {
            var resp = new NotaResponse();

            var cliPedido = await _clienteQuery.GetCliMongoByRelationId(request.ClienteId.ToString());
            var fornPedido = await _fornedorQuery.GetFornecedorMongoByRelationId(request.FornecedorId.ToString());
            var produtosPedido = await _produtoQuery.GetProdutosMongoByRelationsIds(request.NotaItens.Select(x => x.ProdutoId.ToString()));

            var novaNota = new Nota();
            /*fornecedor*/
            novaNota.FornecedorId = new Guid(fornPedido.RelationalId);
            /*cliente*/
            novaNota.ClienteId = new Guid(cliPedido.RelationalId);
            /*produtos*/

            foreach (var pedidoItens in request.NotaItens)
            {
                var novoNotaItem = new NotaItens();
                novoNotaItem.Qtd = pedidoItens.Qtd;
                novoNotaItem.ProdutoId = new Guid(pedidoItens.ProdutoId.ToString());
                novoNotaItem.Price = pedidoItens.Price;
                await _pedidoItensRepository.AddAsync(novoNotaItem);
                novaNota.NotaItens.Add(novoNotaItem);
            }

            await _pedidoRepository.AddAsync(novaNota);
            await _unitOfWork.CommitAsync();
            return resp;
        }
    }
}
