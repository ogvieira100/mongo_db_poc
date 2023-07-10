using Microsoft.EntityFrameworkCore;
using mongo_api.Data.Context;
using mongo_api.Models.Pedidos;
using MongoDB.Driver;

namespace mongo_api.Models.Cliente
{

    public interface IClientesMongoManage
    {

        Task ExecManager(List<Tuple<EntityState, Clientes>> clientes);

    }

    public class ClientesMongoManage : IClientesMongoManage
    {
        readonly IMongoCollection<ClientesMongo> _clientesCollection;
        readonly IMongoCollection<EnderecoMongo> _enderecoCollection;
        public ClientesMongoManage(MongoContext contextMongo)
        {
            _clientesCollection = contextMongo.DB.GetCollection<ClientesMongo>(new ClientesMongo().TableName);
            _enderecoCollection = contextMongo.DB.GetCollection<EnderecoMongo>(new EnderecoMongo().TableName);
        }

        public async Task ExecManager(List<Tuple<EntityState, Clientes>> clientes)
        {

            foreach (var cliente in clientes) {

                switch (cliente.Item1)
                {
                    case EntityState.Deleted:
                        await DeleteAsync(cliente.Item2);
                        break;
                    case EntityState.Modified:
                        await UpdateAsync(cliente.Item2);
                        break;
                    case EntityState.Added:
                        await InsertAsync(cliente.Item2);
                        break;
                    default:
                        break;
                }

            }
        }

        private async Task DeleteAsync(Clientes cli)
        {
            if (cli != null)
            {
                var cliMongoDelete = (await _clientesCollection.FindAsync(x => x.RelationalId == cli.Id.ToString()))?.FirstOrDefault();
                if (cliMongoDelete != null)
                {
                    /*deletar a cascata nesse case endereço não existe sem cliente */
                    foreach (var endMongo in cliMongoDelete.Enderecos)
                    {
                        await _enderecoCollection.DeleteOneAsync(x => x.RelationalId == endMongo.RelationalId);
                    }
                    await _clientesCollection.DeleteOneAsync(x => x.RelationalId == cli.Id.ToString());
                }
            }
        }

        async Task UpdateAsync(Clientes cliente)
        {
            var cli = cliente;
            var cliMongo = (await _clientesCollection.FindAsync(x => x.RelationalId == cli.Id.ToString()))?.FirstOrDefault();
            if (cliMongo is not null
                && cli is not null)
            {
                cliMongo.CPF = cli.CPF;
                cliMongo.RelationalId = cli.Id.ToString();
                cliMongo.Nome = cli.Nome;
                cliMongo.Enderecos = cli
                                    .Enderecos
                                    .Select(x => new EnderecoMongo
                                    {
                                        Logradouro = x.Logradouro,
                                        Estado = x.Estado,
                                        Cliente = new ClientesMongo
                                        {
                                            CPF = cli.CPF,
                                            Nome = cli.Nome
                                        }

                                    }).ToList();

                await _clientesCollection.DeleteOneAsync(x => x.Id == cliMongo.Id);
                await _clientesCollection.InsertOneAsync(cliMongo);

                //var cliMongoId = cliMongo.Id;
                //var atualizacao = Builders<ClientesMongo>.Update.Set(_ => _, cliMongo);
                //await _clientesCollection.UpdateOneAsync(_ => _.Id == cliMongoId, atualizacao);
            }
        }

        async Task InsertAsync( Clientes cliente)
        {
            var clienteMongo = new ClientesMongo();
            clienteMongo.CPF = cliente.CPF;
            clienteMongo.Nome = cliente.Nome;
            clienteMongo.RelationalId = cliente.Id.ToString() ?? "";

            foreach (var end in cliente.Enderecos)
            {
                var endMongo = new EnderecoMongo();
                endMongo.Estado = end.Estado;
                endMongo.Logradouro = end.Logradouro;
                endMongo.RelationalId = end.Id.ToString();

                var cliEndMongo = new ClientesMongo();
                cliEndMongo.CPF = clienteMongo.CPF;
                cliEndMongo.RelationalId = clienteMongo.RelationalId;
                cliEndMongo.Nome = clienteMongo.Nome;
                endMongo.Cliente = cliEndMongo;


                clienteMongo.Enderecos.Add(endMongo);
            }
            await _clientesCollection.InsertOneAsync(clienteMongo);
        }
    }
    public class ClientesMongo : BaseMongo
    {

        public string CPF { get; set; }
        public string Nome { get; set; }
        public List<EnderecoMongo> Enderecos { get; set; }
        public ClientesMongo()
        {
            Enderecos = new List<EnderecoMongo>();
            TableName = "clientes";
        }
    }
    public class Clientes : Base
    {

        public string CPF { get; set; }
        public string Nome { get; set; }
        public virtual List<Endereco> Enderecos { get; set; }
        public virtual IEnumerable<Nota> Notas { get; set; }

        public virtual IEnumerable<Pedido> Pedidos { get; set; }
        public Clientes()
        {
            Enderecos = new List<Endereco>();
            Notas = new List<Nota>();
            Pedidos = new List<Pedido>();   
        }
    }
}
