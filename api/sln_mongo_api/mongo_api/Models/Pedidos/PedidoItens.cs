using Microsoft.EntityFrameworkCore;
using mongo_api.Data.Context;
using mongo_api.Data.Repository;
using mongo_api.Models.Cliente;
using mongo_api.Models.Fornecedores;
using mongo_api.Models.Produto;
using MongoDB.Driver;

namespace mongo_api.Models.Pedidos
{

    public interface IPedidoItensMongoManage
    {

        Task ExecManager(List<Tuple<EntityState, PedidoItens>> pedidos);
    }

    public class PedidoItensMongoManage : IPedidoItensMongoManage
    {


        readonly IClienteQuery _clienteQuery;
        readonly IProdutoQuery _produtoQuery;
        readonly IFornecedorQuery _fornedorQuery;
        readonly IMongoCollection<PedidoMongo> _pedidoCollection;
        readonly IMongoCollection<PedidoItensMongo> _pedidoItemCollection;
        readonly IPedidoQuery _pedidoQuery;
        readonly IPedidoMongoRepository _pedidoMongoRepository;

        public PedidoItensMongoManage(IFornecedorQuery fornedorQuery,
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
            _pedidoItemCollection = contextMongo.DB.GetCollection<PedidoItensMongo>(new PedidoItensMongo().TableName);
        }

        public async Task ExecManager(List<Tuple<EntityState, PedidoItens>> pedidos)
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

        private async Task InsertAsync(PedidoItens item2)
        {

            
            var produtosPedido = await _produtoQuery.GetProdutoMongoByRelationId(item2.ProdutoId.ToString());
            var pedidoMongo  = await _pedidoQuery.GetPedidoUpdateByRelationalId(item2.PedidoId.ToString());    


            var pedidoMongoItem = new PedidoItensMongo();

            pedidoMongoItem.Qtd  =   item2.Qtd;
            pedidoMongoItem.RelationalId = item2.Id.ToString();
            pedidoMongoItem.Price = item2.Price;
            pedidoMongoItem.PedidoId = pedidoMongo.RelationalId.ToString();
            pedidoMongoItem.Pedido = pedidoMongo;
            pedidoMongoItem.ProdutoId = produtosPedido.RelationalId.ToString();
            pedidoMongoItem.Produto = produtosPedido;


            await _pedidoItemCollection.InsertOneAsync(pedidoMongoItem);
        }

        private async Task UpdateAsync(PedidoItens item2)
        {
            await DeleteAsync(item2);
            await InsertAsync(item2); 
           
        }

        private async Task DeleteAsync(PedidoItens item2)
        {
            var pedidoItem  = (await _pedidoItemCollection.FindAsync(x => x.RelationalId == item2.Id.ToString())).FirstOrDefault();
            await _pedidoItemCollection.DeleteOneAsync(x => x.RelationalId == pedidoItem.Id.ToString());
        }
    }
    public class PedidoItensMongo : BaseMongo
    {
        public int Qtd { get; set; }
        public string ProdutoId { get; set; }
        public ProdutoMongo Produto { get; set; }
        public decimal Price { get; set; }
        public string PedidoId { get; set; }
        public PedidoMongo Pedido { get; set; }

        public PedidoItensMongo()
        {
            TableName = "pedidoItens";
        }
    }
    public class PedidoItens : Base
    {

        public int Qtd { get; set; }
        public virtual Guid ProdutoId { get; set; }
        public virtual Produtos Produto { get; set; }
        public decimal Price { get; set; }
        public virtual Guid PedidoId { get; set; }
        public virtual Pedido Pedido { get; set; }

    }
}
