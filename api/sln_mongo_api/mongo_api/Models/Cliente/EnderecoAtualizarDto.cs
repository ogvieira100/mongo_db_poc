namespace mongo_api.Models.Cliente
{
    public class EnderecoAtualizarDto
    {
        public Guid Id { get; set; }
        public string Logradouro { get; set; }
        public UF Estado { get; set; }
    }
}
