using Microsoft.EntityFrameworkCore;
using mongo_api.Data.Context;
using mongo_api.Data.Repository;
using mongo_api.Models.Cliente;
using mongo_api.Models.Fornecedores;
using mongo_api.Models.Produto;
using MongoDB.Driver;

namespace mongo_api.Models.Pedidos
{
    public interface IPedidoMongoManage
    {
        Task ExecManager(List<Tuple<EntityState, Pedido>> pedidos);
    }

    public class PedidoMongoManage : IPedidoMongoManage
    {


        readonly IClienteQuery _clienteQuery;
        readonly IProdutoQuery _produtoQuery;
        readonly IFornecedorQuery _fornedorQuery;
        readonly IMongoCollection<PedidoMongo> _pedidoCollection;
        readonly IPedidoQuery _pedidoQuery;
        readonly IPedidoMongoRepository _pedidoMongoRepository;

        public PedidoMongoManage(IFornecedorQuery fornedorQuery,
                                 IClienteQuery clienteQuery,
                                 IPedidoQuery pedidoQuery,
                                 MongoContext contextMongo,
                                 IPedidoMongoRepository pedidoMongoRepository,
                                 IProdutoQuery produtoQuery)
        {
            _clienteQuery = clienteQuery;
            _produtoQuery = produtoQuery;
            _fornedorQuery = fornedorQuery;
            _pedidoQuery = pedidoQuery;
            _pedidoMongoRepository = pedidoMongoRepository;
            _pedidoCollection = contextMongo.DB.GetCollection<PedidoMongo>(new PedidoMongo().TableName);
        }

        public async Task ExecManager(List<Tuple<EntityState, Pedido>> pedidos)
        {
            foreach (var pedido in pedidos)
            {

                switch (pedido.Item1)
                {
                    case EntityState.Deleted:
                        await DeleteAsync(pedido.Item2);
                        break;
                    case EntityState.Modified:
                        await UpdateAsync(pedido.Item2);
                        break;
                    case EntityState.Added:
                        await InsertAsync(pedido.Item2);
                        break;
                    default:
                        break;
                }

            }
        }

        private async Task InsertAsync(Pedido item2)
        {

            var clienteMongo = await _clienteQuery.GetCliMongoByRelationId(item2.ClienteId.ToString());
            var fornMongo = await _fornedorQuery.GetFornecedorMongoByRelationId(item2.FornecedorId.ToString());
            var produtosPedido = await _produtoQuery.GetProdutosMongoByRelationsIds(item2.PedidoItens.Select(x => x.ProdutoId.ToString()));



            var pedidoMongo = new PedidoMongo();
            pedidoMongo.Observation = item2.Observation ?? "";
            pedidoMongo.FornecedorId = item2.FornecedorId.ToString();
            pedidoMongo.RelationalId = item2.Id.ToString();
            pedidoMongo.ClienteId = item2.ClienteId.ToString();
            pedidoMongo.Cliente = clienteMongo;
            pedidoMongo.Fornecedor = fornMongo;

            foreach (var pedidoItem in item2.PedidoItens)
            {

                var prod = produtosPedido.FirstOrDefault(x => x.RelationalId == pedidoItem.ProdutoId.ToString());
                pedidoMongo.PedidoItens.Add(new PedidoItensMongo
                {
                    Price = pedidoItem.Price,
                    Qtd = pedidoItem.Qtd,
                    ProdutoId = prod.RelationalId,
                    RelationalId = pedidoItem.Id.ToString(),
                    Produto = prod
                });
            }

            await _pedidoCollection.InsertOneAsync(pedidoMongo);
        }

        private async Task UpdateAsync(Pedido item2)
        {
            var pedidoMongoRepository = await _pedidoMongoRepository.GetPedidoUpdateByRelationalId(item2.Id.ToString());
           
            var novoPedidoMongo = new PedidoMongo();
            novoPedidoMongo.ClienteId = item2.ClienteId.ToString();
            novoPedidoMongo.FornecedorId = item2.FornecedorId.ToString();
            novoPedidoMongo.Observation = item2.Observation ?? "";
            novoPedidoMongo.RelationalId = item2.Id.ToString();
            foreach (var pedidoItens in item2.PedidoItens)
            {
                var novoPedidoItenMongo = new PedidoItensMongo();
                    novoPedidoItenMongo.Qtd = pedidoItens.Qtd;
                    novoPedidoItenMongo.ProdutoId = pedidoItens.ProdutoId.ToString();
                    novoPedidoItenMongo.Price = pedidoItens.Price;
                    novoPedidoMongo.PedidoItens.Add(novoPedidoItenMongo);
                novoPedidoItenMongo.RelationalId = pedidoItens.Id.ToString();
            }
            var pedidoItensMongo = pedidoMongoRepository
                                      .PedidoItens
                                      .Where(x => !item2.PedidoItens.Select(ped => ped.Id.ToString())
                                      .Contains(x.RelationalId));
            foreach (var itensMongo in pedidoItensMongo)
                novoPedidoMongo.PedidoItens.Add(itensMongo);

           
            await _pedidoMongoRepository.UpdatePedidoMongo(novoPedidoMongo);
            await DeleteAsync(item2);
            await _pedidoCollection.InsertOneAsync(novoPedidoMongo);
        }

        private async Task DeleteAsync(Pedido item2)
        {
            var pedidoMongo = await _pedidoQuery.GetPedidoUpdateByRelationalId(item2.Id.ToString());
            await _pedidoCollection.DeleteOneAsync(x => x.RelationalId == pedidoMongo.Id.ToString());
        }
    }
    public class PedidoMongo : BaseMongo
    {

        public string ClienteId { get; set; }
        public ClientesMongo Cliente { get; set; }
        public string FornecedorId { get; set; }

        public FornecedorMongo Fornecedor { get; set; }
        public List<PedidoItensMongo> PedidoItens { get; set; }
        public string Observation { get; set; }

        public PedidoMongo()
        {
            PedidoItens = new List<PedidoItensMongo>();
            TableName = "pedido";
        }

    }
    public class Pedido : Base
    {

        public virtual Guid ClienteId { get; set; }
        public virtual Clientes Cliente { get; set; }
        public virtual Guid FornecedorId { get; set; }
        public virtual Fornecedor Fornecedor { get; set; }
        public virtual List<PedidoItens> PedidoItens { get; set; }
        public string? Observation { get; set; }

        public Pedido()
        {
            PedidoItens = new List<PedidoItens>();
        }
    }
}
