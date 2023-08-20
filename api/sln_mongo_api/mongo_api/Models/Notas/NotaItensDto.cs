namespace mongo_api.Models.Notas
{
    public class NotaItensDto
    {
        public Guid? Id { get; set; }
        public int Qtd { get; set; }
        public virtual Guid ProdutoId { get; set; }
        public decimal Price { get; set; }
    }
}
