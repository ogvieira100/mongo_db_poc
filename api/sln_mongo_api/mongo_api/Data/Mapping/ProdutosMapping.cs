using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using mongo_api.Models.Produto;

namespace mongo_api.Data.Mapping
{
    public class ProdutosMapping : IEntityTypeConfiguration<Produtos>
    {
        public void Configure(EntityTypeBuilder<Produtos> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Descricao)
                .HasMaxLength(200); 
                ;

            builder.ToTable("Produto");
        }
    }

}
