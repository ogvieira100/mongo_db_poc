using mongo_api.Models;
using mongo_api.Models.Cliente;
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
                    cm.AddKnownType(typeof(EnderecoMongo));
                    cm.GetMemberMap(x => x.Id)
                   .SetIdGenerator(StringObjectIdGenerator.Instance)
                   .SetSerializer(new StringSerializer(BsonType.ObjectId));

                    cm.MapMember(c => c.RelationalId).SetElementName("relationalId");
                    cm.SetIgnoreExtraElements(true);
                });

            }
            if (!BsonClassMap.IsClassMapRegistered(typeof(EnderecoMongo)))
            {


                BsonClassMap.RegisterClassMap<EnderecoMongo>(i =>
                {
                    i.AutoMap();
                    //i.MapIdMember(x => x.Id).SetElementName("id");
                    //i.GetMemberMap(x => x.Id)
                    //.SetIdGenerator(StringObjectIdGenerator.Instance)
                    //.SetSerializer(new StringSerializer(BsonType.ObjectId));
                    i.MapMember(c => c.Estado).SetSerializer(new EnumSerializer<UF>(BsonType.Int32))
                    .SetElementName("estado");
                    i.MapMember(c => c.Logradouro).SetElementName("logradouro");
                    i.MapMember(x => x.Cliente).SetElementName("cliente");
                    i.SetDiscriminator("enderecos");
                    i.SetIgnoreExtraElements(true);
                    //i.SetIgnoreExtraElements(true);
                });
            }

            if (!BsonClassMap.IsClassMapRegistered(typeof(ClientesMongo)))
            {

                BsonClassMap.RegisterClassMap<ClientesMongo>(i =>
                {
                    i.AutoMap();
                    //i.MapIdMember(x => x.Id).SetElementName("id");
                    i.SetDiscriminator("clientes");
                    // i.GetMemberMap(x => x.Id)
                    //.SetIdGenerator(StringObjectIdGenerator.Instance)
                    //.SetSerializer(new StringSerializer(BsonType.ObjectId));
                    i.MapMember(c => c.CPF).SetElementName("cpf");
                    i.MapMember(c => c.Nome).SetElementName("nome");
                    i.MapMember(c => c.Enderecos).SetElementName("enderecos");
                    //i.MapExtraElementsMember(x => x.Enderecos);
                    i.SetIgnoreExtraElements(true);
                });
            }


        }
    }
}
