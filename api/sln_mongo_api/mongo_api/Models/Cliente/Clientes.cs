namespace mongo_api.Models.Cliente
{

    public class ClientesMongo : BaseMongo
    {

        public string CPF { get; set; }
        public string Nome { get; set; }
        public  List<EnderecoMongo> Enderecos { get; set; }

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

        public Clientes()
        {
            Enderecos = new List<Endereco>();
        }
    }
}
