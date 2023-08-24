using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using mongo_api.Data.Context;
using mongo_api.Data.Repository;
using mongo_api.Models.Cliente;
using mongo_api.Models.Fornecedores;
using mongo_api.Models.Pedidos;
using mongo_api.Models.Produto;
using MongoDB.Driver;

namespace mongo_api.Models.Notas
{


    public interface INotaItensMongoManage
    {

        Task ExecManager(List<Tuple<EntityState, NotaItens>> notaItens);
    }

    public class NotaItensMongoManage : INotaItensMongoManage
    {


        readonly IClienteQuery _clienteQuery;
        readonly IProdutoQuery _produtoQuery;
        readonly IFornecedorQuery _fornedorQuery;
        readonly IMongoCollection<PedidoMongo> _pedidoCollection;
        readonly IMongoCollection<NotaItensMongo> _notaItensCollection;
        readonly INotaQuery _notaQuery;
        readonly IPedidoMongoRepository _pedidoMongoRepository;

        public NotaItensMongoManage(IFornecedorQuery fornedorQuery,
                                 IClienteQuery clienteQuery,
                                 INotaQuery notaQuery,
                                 MongoContext contextMongo,
                                 IPedidoMongoRepository pedidoMongoRepository,
                                 IProdutoQuery produtoQuery)
        {
            _clienteQuery = clienteQuery;
            _produtoQuery = produtoQuery;
            _fornedorQuery = fornedorQuery;
            _notaQuery = notaQuery;
            _pedidoMongoRepository = pedidoMongoRepository;
            _pedidoCollection = contextMongo.DB.GetCollection<PedidoMongo>(new PedidoMongo().TableName);
            _notaItensCollection = contextMongo.DB.GetCollection<NotaItensMongo>(new NotaItensMongo().TableName);
        }

        public async Task ExecManager(List<Tuple<EntityState, NotaItens>> notaItens)
        {
            foreach (var pedido in notaItens)
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

        private async Task InsertAsync(NotaItens item2)
        {


            var produtosPedido = await _produtoQuery.GetProdutoMongoByRelationId(item2.ProdutoId.ToString());
            var notaMongo = await _notaQuery.GetNotaUpdateByRelationalId(item2.NotaId.ToString());


            var notaMongoItem = new NotaItensMongo();

            notaMongoItem.Qtd = item2.Qtd;
            notaMongoItem.RelationalId = item2.Id.ToString();
            notaMongoItem.Price = item2.Price;
            notaMongoItem.NotaId = notaMongo.RelationalId.ToString();

            notaMongoItem.ProdutoId = produtosPedido.RelationalId.ToString();
            notaMongoItem.Produto = produtosPedido;


            await _notaItensCollection.InsertOneAsync(notaMongoItem);
        }

        private async Task UpdateAsync(NotaItens item2)
        {
            await DeleteAsync(item2);
            await InsertAsync(item2);

        }

        private async Task DeleteAsync(NotaItens item2)
        {
            var pedidoItem = (await _notaItensCollection.FindAsync(x => x.RelationalId == item2.Id.ToString())).FirstOrDefault();
            await _notaItensCollection.DeleteOneAsync(x => x.RelationalId == pedidoItem.Id.ToString());
        }
    }

    public class NotaItensMongo : BaseMongo
    {

       


        [JsonPropertyName("qtd")]
        public int Qtd { get; set; }

        [JsonPropertyName("produtoId")]
        public string ProdutoId { get; set; }

        [JsonPropertyName("produto")]
        public ProdutoMongo Produto { get; set; }

        [JsonPropertyName("price")]
        public decimal Price { get; set; }

        [JsonPropertyName("notaId")]
        public string NotaId { get; set; }



        public NotaItensMongo()
        {
            TableName = "notaitens";
        }

    }
    public class NotaItens : Base
    {
        public int Qtd { get; set; }
        public virtual Guid ProdutoId { get; set; }
        public virtual Produtos Produto { get; set; }
        public decimal Price { get; set; }
        public virtual Guid NotaId { get; set; }
        public virtual Nota Nota { get; set; }

    }
}
