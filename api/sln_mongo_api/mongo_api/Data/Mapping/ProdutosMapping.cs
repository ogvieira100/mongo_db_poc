using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using mongo_api.Models;

namespace mongo_api.Data.Mapping
{
    public class ProdutosMapping : IEntityTypeConfiguration<Produto>
    {
        public void Configure(EntityTypeBuilder<Produto> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Descricao)
                .HasMaxLength(200); 
                ;

            builder.ToTable("Produto");
        }
    }

}
