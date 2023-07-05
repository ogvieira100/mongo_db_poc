namespace mongo_api.Models
{

    public class ProdutoMongo : BaseMongo
    {
        public string Descricao { get; set; }

    }
    public class Produto:Base
    {
        public string Descricao { get; set; }

        public virtual IEnumerable<PedidoItens> PedidoItens { get; set; }

        public Produto()
        {
            PedidoItens = new List<PedidoItens>();  
        }

    }
}
