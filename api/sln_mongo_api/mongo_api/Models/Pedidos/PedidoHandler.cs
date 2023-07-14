using MediatR;
using mongo_api.Data.Repository;
using mongo_api.Models.Cliente;
using mongo_api.Models.Fornecedores;
using mongo_api.Models.Produto;

namespace mongo_api.Models.Pedidos
{
    public class PedidoHandler :
        IRequestHandler<PedidoInserirCommand, PedidoResponse>,
        IRequestHandler<PedidoAtualizarCommand, PedidoResponse>,
        IRequestHandler<PedidoDeletarCommand, PedidoResponse>
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

        public PedidoHandler(IFornecedorQuery fornedorQuery,
                             IUnitOfWork unitOfWork,
                             IPedidoQuery pedidoQuery,
                             IProdutoQuery produtoQuery,
                             IPedidoMongoRepository pedidoMongoRepository,
                             IBaseRepository<Pedido> pedidoRepository,
                             IBaseRepository<PedidoItens> pedidoItensRepository,
                             IBaseRepository<Clientes> clienteRepository,
                             IBaseRepository<Fornecedor> fornecedorRepository,
                             IClienteQuery clienteQuery)
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
        }
        public async Task<PedidoResponse> Handle(PedidoInserirCommand request,
            CancellationToken cancellationToken)
        {
            var resp = new PedidoResponse();

            var cliPedido = await _clienteQuery.GetCliMongoByRelationId(request.ClienteId.ToString());
            var fornPedido = await _fornedorQuery.GetFornecedorMongoByRelationId(request.FornecedorId.ToString());
            var produtosPedido = await _produtoQuery.GetProdutosMongoByRelationsIds(request.PedidoItensDto.Select(x => x.ProdutoId.ToString()));

            var novoPedido = new Pedido();
            /*fornecedor*/
            novoPedido.FornecedorId = new Guid(fornPedido.RelationalId);
            /*cliente*/
            novoPedido.ClienteId = new Guid(cliPedido.RelationalId);
            /*produtos*/

            foreach (var pedidoItens in request.PedidoItensDto)
            {
                var novoPedidoItem = new PedidoItens();
                novoPedidoItem.Qtd = pedidoItens.Qtd;
                novoPedidoItem.ProdutoId = new Guid(pedidoItens.ProdutoId.ToString());
                novoPedidoItem.Price = pedidoItens.Price;
                await _pedidoItensRepository.AddAsync(novoPedidoItem);
                novoPedido.PedidoItens.Add(novoPedidoItem);
            }

            await _pedidoRepository.AddAsync(novoPedido);
            await _unitOfWork.CommitAsync();
            return resp;
        }

        public async Task<PedidoResponse> Handle(PedidoAtualizarCommand request, CancellationToken cancellationToken)
        {
            var resp = new PedidoResponse();
            var pedidoMongo  =  await _pedidoQuery.GetPedidoUpdateByRelationalId(request.Id.ToString());
            var pedido = new Pedido();
            pedido.FornecedorId = new Guid(request.FornecedorId.ToString());
            pedido.ClienteId = new Guid(request.ClienteId.ToString());
            pedido.Observation = request.Observation;
            pedido.Id = request.Id;

            foreach (var pedidoItemRequest in request.PedidoItensDto)
            {
                var pedidoItem = new PedidoItens();
                if (pedidoItemRequest.Id.HasValue)
                {
                    pedidoItem.Id = pedidoItemRequest.Id.Value;
                    var pedidoItemMongo = pedidoMongo.PedidoItens?.FirstOrDefault(x => x.RelationalId == pedidoItem.Id.ToString());
                    if (pedidoItemMongo is not null)
                    {
                         pedidoItem.ProdutoId = new Guid(pedidoItemMongo.ProdutoId);
                        _pedidoItensRepository.Update(pedidoItem);
                    }
                    else
                        await _pedidoItensRepository.AddAsync(pedidoItem);
                }
                else
                    await _pedidoItensRepository.AddAsync(pedidoItem);
                pedidoItem.Qtd = pedidoItemRequest.Qtd;
                pedidoItem.Price = pedidoItemRequest.Price;
                pedido.PedidoItens.Add(pedidoItem);
            }

            _pedidoItensRepository.Update(pedido);
            await _unitOfWork.CommitAsync();  
            return resp;
        }

        public Task<PedidoResponse> Handle(PedidoDeletarCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

    }
}
