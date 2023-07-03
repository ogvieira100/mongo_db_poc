namespace mongo_api.Models.Cliente
{
    public enum UF
    {
        SP = 1,
        RJ = 2,
        MG = 3
    }

    public class EnderecoMongo:BaseMongo
    {
       
        public string Logradouro { get; set; }

        public string ClienteRelationalId { get; set; }

        public  ClientesMongo Cliente { get; set; }
        public UF Estado { get; set; }

        public EnderecoMongo()
        {
            Cliente = new ClientesMongo();
            TableName = "endereco";
        }
    }
    public class Endereco : Base
    {
        public virtual Guid ClienteId { get; set; }

        public virtual Clientes Cliente { get; set; }

        public string Logradouro { get; set; }

        public UF Estado { get; set; }

        public Endereco():base()
        {
            
        }


    }
}
