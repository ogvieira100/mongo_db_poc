using Microsoft.EntityFrameworkCore;
using mongo_api.Data.Context;
using mongo_api.Models.Cliente;
using MongoDB.Driver;

namespace mongo_api.Data.Repository
{
    public class UnitOfWork : IUnitOfWork
    {

        readonly AplicationContext _aplicationContext;
        readonly MongoContext _mongoContext;
        IMongoCollection<ClientesMongo> _clientesCollection;
        IMongoCollection<EnderecoMongo> _enderecosCollection;
        public UnitOfWork(AplicationContext aplicationContext, MongoContext mongoContext)
        {
            _aplicationContext = aplicationContext;
            _mongoContext = mongoContext;
            _clientesCollection = _mongoContext.DB.GetCollection<ClientesMongo>("clientes");
            _enderecosCollection = _mongoContext.DB.GetCollection<EnderecoMongo>("endereco");
        }

  
        public async Task<bool> CommitAsync()
        {

            var res = false;
            List<Tuple<EntityState, Type, object>> entrys
                
                = new List<Tuple<EntityState, Type, object>>();



            foreach (var entry in _aplicationContext.ChangeTracker.Entries())
            {
                var baseEntry = entry.Entity;
                entrys.Add(new Tuple<EntityState, Type, object>(entry.State,
                                                                baseEntry.GetType(),
                                                                baseEntry));
            }

            res = await _aplicationContext.SaveChangesAsync() > 0;
            if (res)
            {
                foreach (var item in entrys)
                {
                    if (item.Item1 == EntityState.Added)
                    {
                        if (item.Item2 == typeof(Clientes))
                        {
                            var cli = item.Item3 as Clientes;
                           
                            var clienteMongo = new ClientesMongo();
                            clienteMongo.CPF = cli.CPF;
                            clienteMongo.Nome = cli.Nome;
                            clienteMongo.RelationalId = cli.Id.ToString() ?? "";

                            foreach (var end in cli.Enderecos)
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
                        if (item.Item2 == typeof(Endereco))
                        {
                            var end = item.Item3 as Endereco;
                            var enderecoMongo = new EnderecoMongo();
                            enderecoMongo.Estado = end.Estado;
                            enderecoMongo.Logradouro = end.Logradouro;
                            enderecoMongo.RelationalId = end.Id.ToString();
                            enderecoMongo.ClienteRelationalId = end.Cliente.Id.ToString();  
                            if (end.Cliente is not null)
                                enderecoMongo.Cliente = new ClientesMongo
                                {
                                    CPF = end.Cliente.CPF,
                                    RelationalId = end.Id.ToString(),
                                    Nome = end.Cliente.Nome
                                };

                            await _enderecosCollection.InsertOneAsync(enderecoMongo);
                        }
                    }
                    if (item.Item1 == EntityState.Modified)
                    {

                        if (item.Item2 == typeof(Clientes))
                        {
                            var cli = item.Item3 as Clientes;
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
                        if (item.Item2 == typeof(Endereco))
                        {
                            var end = item.Item3 as Endereco;

                            if (end != null)
                            {
                                var cliEnd = end.Cliente;
                                var endMongo = (await _enderecosCollection.FindAsync(x => x.RelationalId == end.Id.ToString()))?.FirstOrDefault();
                                if (endMongo is not null)
                                {
                                    endMongo.Logradouro = end.Logradouro;
                                    endMongo.Estado = end.Estado;
                                    endMongo.RelationalId = end.Id.ToString();
                                    endMongo.Cliente = new ClientesMongo
                                    {
                                        CPF = cliEnd.CPF,
                                        Nome = cliEnd.Nome,
                                        RelationalId = cliEnd.Id.ToString()
                                    };

                                    await _enderecosCollection.DeleteOneAsync(x => x.Id == endMongo.Id);
                                    await _enderecosCollection.InsertOneAsync(endMongo);

                                    //var endMongoId = endMongo.Id;
                                    //var atualizacao = Builders<EnderecoMongo>.Update.Set(_ => _, endMongo);
                                    //await _enderecosCollection.UpdateOneAsync(_ => _.Id == endMongoId, atualizacao);
                                }
                            }
                        }
                    }
                    if (item.Item1 == EntityState.Deleted)
                    {
                        if (item.Item2 == typeof(Clientes))
                        {
                            var cli = item.Item3 as Clientes;
                            if (cli != null)
                            {
                                var cliMongoDelete = (await _clientesCollection.FindAsync(x => x.RelationalId == cli.Id.ToString()))?.FirstOrDefault();
                                if (cliMongoDelete != null)
                                {
                                    /*deletar a cascata nesse case endereço não existe sem cliente */
                                    foreach (var endMongo in cliMongoDelete.Enderecos)
                                    {
                                        await _enderecosCollection.DeleteOneAsync(x => x.RelationalId == endMongo.RelationalId);
                                    }
                                   
                                    await _clientesCollection.DeleteOneAsync(x => x.RelationalId == cli.Id.ToString());
                                }
                            }
                        }
                        if (item.Item2 == typeof(Endereco))
                        {
                            var end = item.Item3 as Endereco;
                            if (end != null)
                            {
                                var enderecoMongoDelete = (await _enderecosCollection.FindAsync(x => x.RelationalId == end.Id.ToString()))?.FirstOrDefault();
                                if (enderecoMongoDelete != null)
                                {
                                  
                                    
                                    await _enderecosCollection.DeleteOneAsync(x => x.RelationalId == enderecoMongoDelete.Id.ToString());
                                }
                            }

                        }
                    }
                }

            }
            /*fazer as alterações aqui*/

            return res;

        }
        public void Dispose() => GC.SuppressFinalize(this);
    }
}
