using mongo_api.Models.Cliente;

namespace mongo_api.Models
{
    public class PedidoMongo:BaseMongo
    {

        public string ClienteId { get; set; }
        public ClientesMongo Cliente { get; set; }
        public string FornecedorId { get; set; }

        public FornecedorMongo Fornecedor { get; set; }
        public List<PedidoItensMongo> PedidoItens { get; set; }
        public string Observation { get; set; }

        public PedidoMongo()
        {
            PedidoItens = new List<PedidoItensMongo>();
            TableName = "pedido";
        }

    }
    public class Pedido:Base
    {

        public virtual Guid ClienteId { get; set; }
        public virtual mongo_api.Models.Cliente.Clientes Cliente { get; set; }
        public virtual Guid FornecedorId { get; set; }
        public virtual Fornecedor Fornecedor { get; set; }
        public virtual IEnumerable<PedidoItens> PedidoItens { get; set; }
        public string Observation { get; set; }

        public Pedido()
        {
            PedidoItens = new List<PedidoItens>();  
        }
    }
}
