using mongo_api.Models;
using mongo_api.Models.Cliente;
using mongo_api.Models.Fornecedores;
using mongo_api.Models.Notas;
using mongo_api.Models.Pedidos;
using mongo_api.Models.Produto;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;

namespace mongo_api.Data.Context
{
    public class MongoContext
    {

        public IMongoDatabase DB { get; }

        public MongoContext(IConfiguration configuration)
        {
            try
            {
                var client = new MongoClient(configuration.GetSection("ConnectionStrings:MongoDb").Value);
                DB = client.GetDatabase(configuration.GetSection("NomeBancoMongoDb").Value);
                MapClasses();
            }
            catch (Exception ex)
            {
                throw new MongoException("Não foi possivel se conectar ao MongoDB", ex);
            }
        }

        void MapClasses()
        {
            if (!BsonClassMap.IsClassMapRegistered(typeof(BaseMongo)))
            {
                BsonClassMap.RegisterClassMap<BaseMongo>(cm =>
                {
                    cm.AutoMap();
                    cm.SetIsRootClass(true);
                    cm.GetMemberMap(x => x.Id)
                   .SetIdGenerator(StringObjectIdGenerator.Instance)
                   .SetSerializer(new StringSerializer(BsonType.ObjectId));

                    cm.MapMember(c => c.RelationalId).SetElementName("relationalId");
                    cm.SetIgnoreExtraElements(true);
                });

            }

            if (!BsonClassMap.IsClassMapRegistered(typeof(ProdutoMongo)))
            {
                BsonClassMap.RegisterClassMap<ProdutoMongo>(i =>
                {
                    i.AutoMap();
                    i.SetDiscriminator(new ProdutoMongo().TableName);
                    i.MapMember(c => c.Descricao).SetElementName("descricao");
                    i.SetIgnoreExtraElements(true);
                });
            }

            if (!BsonClassMap.IsClassMapRegistered(typeof(EnderecoMongo)))
            {
                BsonClassMap.RegisterClassMap<EnderecoMongo>(i =>
                {
                    i.AutoMap();
                    i.MapMember(c => c.Estado).SetSerializer(new EnumSerializer<UF>(BsonType.Int32))
                    .SetElementName("estado");
                    i.MapMember(c => c.Logradouro).SetElementName("logradouro");
                    i.MapMember(x => x.Cliente).SetElementName("cliente");
                    i.MapMember(x => x.ClienteId).SetElementName("clienteId");
                    i.SetDiscriminator(new EnderecoMongo().TableName);
                    i.SetIgnoreExtraElements(true);
                });
            }

            if (!BsonClassMap.IsClassMapRegistered(typeof(ClientesMongo)))
            {
                BsonClassMap.RegisterClassMap<ClientesMongo>(i =>
                {
                    i.AutoMap();
                    i.SetDiscriminator(new ClientesMongo().TableName);
                    i.MapMember(c => c.CPF).SetElementName("cpf");
                    i.MapMember(c => c.Nome).SetElementName("nome");
                    i.MapMember(c => c.Enderecos).SetElementName("enderecos");
                    i.SetIgnoreExtraElements(true);
                });
            }

            if (!BsonClassMap.IsClassMapRegistered(typeof(FornecedorMongo)))
            {
                BsonClassMap.RegisterClassMap<FornecedorMongo>(i =>
                {
                    i.AutoMap();
                    
                    i.SetDiscriminator(new FornecedorMongo().TableName);
                    i.MapMember(c => c.CNPJ).SetElementName("cnpj");
                    i.MapMember(c => c.RazaoSocial).SetElementName("razaoSocial");
                    i.SetIgnoreExtraElements(true);
                });
            }

            if (!BsonClassMap.IsClassMapRegistered(typeof(NotaMongo)))
            {
                BsonClassMap.RegisterClassMap<NotaMongo>(i =>
                {
                    i.AutoMap();
                    
                    i.SetDiscriminator(new NotaMongo().TableName);
                    i.MapMember(c => c.Fornecedor).SetElementName("fornecedor");
                    i.MapMember(c => c.FornecedorId).SetElementName("fornecedorId");
                    i.MapMember(c => c.Cliente).SetElementName("cliente");
                    i.MapMember(c => c.ClienteId).SetElementName("clienteId");
                    i.MapMember(c => c.Observation).SetElementName("observacao");
                    i.MapMember(c => c.Numero).SetElementName("numero");
                    i.MapMember(c => c.NotaItens).SetElementName("notaItens");
                    i.SetIgnoreExtraElements(true);
                });
            }

            if (!BsonClassMap.IsClassMapRegistered(typeof(NotaItensMongo)))
            {
                BsonClassMap.RegisterClassMap<NotaItensMongo>(i =>
                {
                    i.AutoMap();
                    i.MapMember(x => x.Qtd).SetElementName("qtd");
        
                    i.MapMember(x => x.NotaId).SetElementName("notaId");
                    i.MapMember(x => x.Price).SetElementName("price");
                    i.MapMember(x => x.Produto).SetElementName("produto");
                    i.MapMember(x => x.ProdutoId).SetElementName("produtoId");
                    i.SetDiscriminator(new NotaItensMongo().TableName);
                    i.SetIgnoreExtraElements(true);
                });
            }

            if (!BsonClassMap.IsClassMapRegistered(typeof(PedidoMongo)))
            {
                BsonClassMap.RegisterClassMap<PedidoMongo>(i =>
                {
                    i.AutoMap();
                    i.MapMember(x => x.ClienteId).SetElementName("clienteId");
                    i.MapMember(x => x.Cliente).SetElementName("cliente");
                    i.MapMember(x => x.Observation).SetElementName("observacao");
                    i.MapMember(x => x.FornecedorId).SetElementName("fornecedorId");
                    i.MapMember(x => x.Fornecedor).SetElementName("fornecedor");
                    i.MapMember(x => x.PedidoItens).SetElementName("pedidoItens");
                    i.SetDiscriminator(new PedidoMongo().TableName);
                    i.SetIgnoreExtraElements(true);
                    
                });
            }

            if (!BsonClassMap.IsClassMapRegistered(typeof(PedidoItensMongo)))
            {
                BsonClassMap.RegisterClassMap<PedidoItensMongo>(i =>
                {
                    i.AutoMap();
                    i.MapProperty(x => x.Qtd).SetElementName("qtd");
                    i.MapProperty(x=> x.Pedido).SetElementName("pedido");
                    i.MapProperty(x => x.PedidoId).SetElementName("pedidoId");
                    i.MapProperty(x => x.Price).SetElementName("price");
                    i.MapProperty(x => x.Produto).SetElementName("produto");
                    i.MapProperty(x => x.ProdutoId).SetElementName("produtoId");
                    i.SetDiscriminator(new PedidoItensMongo().TableName);
                    i.SetIgnoreExtraElements(true);
                });
            }

       
        }
    }
}
