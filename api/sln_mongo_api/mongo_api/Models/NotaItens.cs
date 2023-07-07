namespace mongo_api.Models
{

    public class NotaItensMongo : BaseMongo
    {

        public int Qtd { get; set; }
        public string ProdutoId { get; set; }
        public ProdutoMongo Produto { get; set; }
        public decimal Price { get; set; }
        public string NotaId { get; set; }
        public NotaMongo Nota { get; set; }

        public NotaItensMongo()
        {
            TableName = "notaItens";
        }

    }
    public class NotaItens:Base
    {
        public int Qtd { get; set; }
        public virtual Guid ProdutoId { get; set; }
        public virtual Produto Produto { get; set; }
        public decimal Price { get; set; }
        public virtual Guid NotaId { get; set; }
        public virtual Nota Nota { get; set; }

    }
}
