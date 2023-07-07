namespace mongo_api.Models
{

    public class FornecedorMongo:BaseMongo
    {
        public string CNPJ { get; set; }
        public string RazaoSocial { get; set; }

        public FornecedorMongo()
        {
            TableName = "fornecedor";
        }


    }
    public class Fornecedor:Base
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
