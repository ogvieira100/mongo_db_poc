using mongo_api.Models.Cliente;
using mongo_api.Models.Fornecedores;

namespace mongo_api.Models
{

    public class NotaMongo : BaseMongo
    {
        public string FornecedorId { get; set; }
        public FornecedorMongo Fornecedor { get; set; }
        public string ClienteId { get; set; }
        public ClientesMongo Cliente { get; set; }
        public List<NotaItensMongo> NotaItens { get; set; } 
        public string Observation { get; set; }
        public string Numero { get; set; }

        public NotaMongo()
        {
            NotaItens = new List<NotaItensMongo>();
            TableName = "nota";
        }
    }

   
    public class Nota:Base
    {

        public virtual Guid ClienteId { get; set; }
        public virtual mongo_api.Models.Cliente.Clientes Cliente { get; set; }
        public virtual Guid FornecedorId { get; set; }
        public virtual Fornecedor Fornecedor { get; set; }
        public virtual IEnumerable<NotaItens> NotaItens { get; set; }
        public string? Observation { get; set; }
        public string Numero { get; set; }

        public Nota()
        {
            NotaItens = new List<NotaItens>();
        }

    }
}
