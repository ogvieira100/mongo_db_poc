using Microsoft.EntityFrameworkCore;
using mongo_api.Data.Context;
using mongo_api.Models.Cliente;
using mongo_api.Models.Fornecedores;
using mongo_api.Models.Produto;
using MongoDB.Driver;

namespace mongo_api.Models.Pedidos
{
    public interface IPedidoMongoManage {

        Task ExecManager(List<Tuple<EntityState, Pedido>> pedidos);
    }

    public class PedidoMongoManage : IPedidoMongoManage
    {


        readonly IClienteQuery _clienteQuery;
        readonly IProdutoQuery _produtoQuery;
        readonly IFornecedorQuery _fornedorQuery;
        readonly IMongoCollection<PedidoMongo> _pedidoCollection;

        public PedidoMongoManage(IFornecedorQuery fornedorQuery,
                                 IClienteQuery clienteQuery,
                                 MongoContext contextMongo, 
                                 IProdutoQuery produtoQuery)
        {
            _clienteQuery = clienteQuery;
            _produtoQuery = produtoQuery;
            _fornedorQuery = fornedorQuery;
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

        private Task UpdateAsync(Pedido item2)
        {
            throw new NotImplementedException();
        }

        private Task DeleteAsync(Pedido item2)
        {
            throw new NotImplementedException();
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
