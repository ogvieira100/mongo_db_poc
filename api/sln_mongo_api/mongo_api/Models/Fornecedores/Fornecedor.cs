using Microsoft.EntityFrameworkCore;
using mongo_api.Data.Context;
using mongo_api.Models.Notas;
using mongo_api.Models.Pedidos;
using mongo_api.Models.Produto;
using MongoDB.Driver;

namespace mongo_api.Models.Fornecedores
{


    public interface IFornecedorMongoManage
    {
        Task ExecManager(List<Tuple<EntityState, Fornecedor>>  fornecedores);
    }

    public class FornecedorMongoManage : IFornecedorMongoManage
    {
        readonly IMongoCollection<FornecedorMongo> _fornecedorCollection;

        public FornecedorMongoManage(MongoContext contextMongo)
        {
            _fornecedorCollection = contextMongo.DB.GetCollection<FornecedorMongo>(new FornecedorMongo().TableName);

        }

        public async Task ExecManager(List<Tuple<EntityState, Fornecedor>> fornecedores)
        {

            foreach (var produto in fornecedores)
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

        private async Task DeleteAsync(Fornecedor forn)
        {
            if (forn != null)
            {
                var fornMongoDelete = (await _fornecedorCollection.FindAsync(x => x.RelationalId == forn.Id.ToString()))?.FirstOrDefault();
                if (fornMongoDelete != null)
                    await _fornecedorCollection.DeleteOneAsync(x => x.RelationalId == forn.Id.ToString());
            }
        }

        async Task UpdateAsync(Fornecedor fornecedor)
        {
            var forn = fornecedor;
            var fornecedorMongo = (await _fornecedorCollection.FindAsync(x => x.RelationalId == forn.Id.ToString()))?.FirstOrDefault();
            if (fornecedorMongo is not null
                && forn is not null)
            {
                fornecedorMongo.CNPJ = forn.CNPJ;
                fornecedorMongo.RazaoSocial = forn.RazaoSocial;
                fornecedorMongo.RelationalId = forn.Id.ToString();


                await _fornecedorCollection.DeleteOneAsync(x => x.Id == fornecedorMongo.Id);
                await _fornecedorCollection.InsertOneAsync(fornecedorMongo);

                //var cliMongoId = cliMongo.Id;
                //var atualizacao = Builders<ClientesMongo>.Update.Set(_ => _, cliMongo);
                //await _clientesCollection.UpdateOneAsync(_ => _.Id == cliMongoId, atualizacao);
            }
        }

        async Task InsertAsync(Fornecedor fornecedor)
        {
            var fornecedorMongo = new FornecedorMongo();
            fornecedorMongo.CNPJ = fornecedor.CNPJ;
            fornecedorMongo.RazaoSocial = fornecedor.RazaoSocial;
            fornecedorMongo.RelationalId = fornecedor.Id.ToString() ?? "";
            await _fornecedorCollection.InsertOneAsync(fornecedorMongo);
        }
    }

    public class FornecedorMongo : BaseMongo
    {
        public string CNPJ { get; set; }
        public string RazaoSocial { get; set; }

        public FornecedorMongo()
        {
            TableName = "fornecedor";
        }


    }
    public class Fornecedor : Base
    {

        public string CNPJ { get; set; }

        public string RazaoSocial { get; set; }

        public virtual IEnumerable<Nota> Notas { get; set; }

        public virtual IEnumerable<Pedido> Pedidos { get; set; }

        public Fornecedor()
        {
            Notas = new List<Nota>();
            Pedidos = new List<Pedido>();
        }

    }
}
