namespace mongo_api.Models
{
    public class Pedido:Base
    {

        public virtual Guid ClienteId { get; set; }

        public virtual mongo_api.Models.Cliente.Clientes Cliente { get; set; }

        public virtual Guid FornecedorId { get; set; }

        public virtual Fornecedor Fornecedor { get; set; }




    }
}
