namespace mongo_api.Models
{

    public class FornecedorMongo:BaseMongo
    {
        public string CNPJ { get; set; }
        public string RazaoSocial { get; set; }


    }
    public class Fornecedor:Base
    {

        public string CNPJ { get; set; }

        public string RazaoSocial { get; set; }

        public virtual IEnumerable<NotaItens> Notas { get; set; }

        public Fornecedor()
        {
            Notas = new List<NotaItens>();   
        }

    }
}
