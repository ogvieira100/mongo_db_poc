using mongo_api.Models.Produto;

namespace mongo_api.Models.Pedidos
{
    public class PedidoItensDto
    {
        public Guid? Id { get; set; }
        public int Qtd { get; set; }
        public virtual Guid ProdutoId { get; set; }
        public decimal Price { get; set; }
       
    }
}
