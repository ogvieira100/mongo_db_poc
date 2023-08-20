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

    public interface INotaMongoManage
    {
        Task ExecManager(List<Tuple<EntityState, Nota>> pedidos);
    }

    public class NotaMongoManage : INotaMongoManage
    {


        readonly IClienteQuery _clienteQuery;
        readonly IProdutoQuery _produtoQuery;
        readonly IFornecedorQuery _fornedorQuery;
        readonly IMongoCollection<NotaMongo> _notaCollection;
        readonly INotaQuery _notaQuery;
        readonly IPedidoMongoRepository _pedidoMongoRepository;

        public NotaMongoManage(IFornecedorQuery fornedorQuery,
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
            _notaCollection = contextMongo.DB.GetCollection<NotaMongo>(new NotaMongo().TableName);
        }

        public async Task ExecManager(List<Tuple<EntityState, Nota>> notas)
        {
            foreach (var nota in notas)
            {

                switch (nota.Item1)
                {
                    case EntityState.Deleted:
                        await DeleteAsync(nota.Item2);
                        break;
                    case EntityState.Added:
                        await InsertAsync(nota.Item2);
                        break;
                    default:
                        break;
                }

            }
        }

        private async Task InsertAsync(Nota item2)
        {

            var clienteMongo = await _clienteQuery.GetCliMongoByRelationId(item2.ClienteId.ToString());
            var fornMongo = await _fornedorQuery.GetFornecedorMongoByRelationId(item2.FornecedorId.ToString());
            var produtosPedido = await _produtoQuery.GetProdutosMongoByRelationsIds(item2.NotaItens.Select(x => x.ProdutoId.ToString()));



            var notaMongo = new NotaMongo();
            notaMongo.Observation = item2.Observation ?? "";
            notaMongo.FornecedorId = item2.FornecedorId.ToString();
            notaMongo.RelationalId = item2.Id.ToString();
            notaMongo.ClienteId = item2.ClienteId.ToString();
            notaMongo.Cliente = clienteMongo;
            notaMongo.Fornecedor = fornMongo;

            foreach (var pedidoItem in item2.NotaItens)
            {

                var prod = produtosPedido.FirstOrDefault(x => x.RelationalId == pedidoItem.ProdutoId.ToString());
                notaMongo.NotaItens.Add(new NotaItensMongo
                {
                    Price = pedidoItem.Price,
                    Qtd = pedidoItem.Qtd,
                    ProdutoId = prod.RelationalId,
                    RelationalId = pedidoItem.Id.ToString(),
                    Produto = prod
                });
            }

            await _notaCollection.InsertOneAsync(notaMongo);
        }

        private async Task DeleteAsync(Nota item2)
        {
            var pedidoMongo = await _notaQuery.GetNotaUpdateByRelationalId(item2.Id.ToString());
            await _notaCollection.DeleteOneAsync(x => x.RelationalId == pedidoMongo.Id.ToString());
        }
    }

    public class NotaMongo : BaseMongo
    {
        public string FornecedorId { get; set; }
        public FornecedorMongo Fornecedor { get; set; }
        public string ClienteId { get; set; }
        public ClientesMongo Cliente { get; set; }
        public List<NotaItensMongo> NotaItens { get; set; }
        public string Observation { get; set; }
        public string Numero { get; set; }

        public NotaMongo()
        {
            NotaItens = new List<NotaItensMongo>();
            TableName = "nota";
        }
    }


    public class Nota : Base
    {

        public virtual Guid ClienteId { get; set; }
        public virtual Clientes Cliente { get; set; }
        public virtual Guid FornecedorId { get; set; }
        public virtual Fornecedor Fornecedor { get; set; }
        public virtual List<NotaItens> NotaItens { get; set; }
        public string? Observation { get; set; }
        public string Numero { get; set; }

        public Nota()
        {
            NotaItens = new List<NotaItens>();
        }

    }
}
