using Microsoft.EntityFrameworkCore;
using mongo_api.Data.Context;
using mongo_api.Models.Cliente;
using mongo_api.Models.Notas;
using mongo_api.Models.Pedidos;
using MongoDB.Driver;

namespace mongo_api.Models.Produto
{

    public interface IProdutoMongoManage
    {
        Task ExecManager(List<Tuple<EntityState, Produtos>> produtos);

    }

    public class ProdutoMongoManage : IProdutoMongoManage
    {
        readonly IMongoCollection<ProdutoMongo> _produtoCollection;
        
        public ProdutoMongoManage(MongoContext contextMongo)
        {
            _produtoCollection = contextMongo.DB.GetCollection<ProdutoMongo>(new ProdutoMongo().TableName);
            
        }

        public async Task ExecManager(List<Tuple<EntityState, Produtos>> produtos)
        {

            foreach (var produto in produtos)
            {

                switch (produto.Item1)
                {
                    case EntityState.Deleted:
                        await DeleteAsync(produto.Item2);
                        break;
                    case EntityState.Modified:
                        await UpdateAsync(produto.Item2);
                        break;
                    case EntityState.Added:
                        await InsertAsync(produto.Item2);
                        break;
                    default:
                        break;
                }

            }
        }

        private async Task DeleteAsync(Produtos cli)
        {
            if (cli != null)
            {
                var cliMongoDelete = (await _produtoCollection.FindAsync(x => x.RelationalId == cli.Id.ToString()))?.FirstOrDefault();
                if (cliMongoDelete != null)
                    await _produtoCollection.DeleteOneAsync(x => x.RelationalId == cli.Id.ToString());

                
            }
        }

        async Task UpdateAsync(Produtos produto)
        {
            var prod = produto;
            var produtoMongo = (await _produtoCollection.FindAsync(x => x.RelationalId == prod.Id.ToString()))?.FirstOrDefault();
            if (produtoMongo is not null
                && prod is not null)
            {
                produtoMongo.Descricao = prod.Descricao;
                produtoMongo.RelationalId = prod.Id.ToString();
               

                await _produtoCollection.DeleteOneAsync(x => x.Id == produtoMongo.Id);
                await _produtoCollection.InsertOneAsync(produtoMongo);

                //var cliMongoId = cliMongo.Id;
                //var atualizacao = Builders<ClientesMongo>.Update.Set(_ => _, cliMongo);
                //await _clientesCollection.UpdateOneAsync(_ => _.Id == cliMongoId, atualizacao);
            }
        }

        async Task InsertAsync(Produtos produto)
        {
            var produtoMongo = new ProdutoMongo();
            produtoMongo.Descricao = produto.Descricao;
            produtoMongo.RelationalId = produto.Id.ToString() ?? "";
            await _produtoCollection.InsertOneAsync(produtoMongo);
        }
    }
    public class ProdutoMongo : BaseMongo
    {
        public string Descricao { get; set; }

        public ProdutoMongo()
        {
            TableName = "produto";
        }

    }
    public class Produtos : Base
    {
        public string Descricao { get; set; }

        public virtual IEnumerable<PedidoItens> PedidoItens { get; set; }

        public virtual IEnumerable<NotaItens> NotaItens { get; set; }

        public Produtos()
        {
            NotaItens = new List<NotaItens>();
            PedidoItens = new List<PedidoItens>();
        }

    }
}
