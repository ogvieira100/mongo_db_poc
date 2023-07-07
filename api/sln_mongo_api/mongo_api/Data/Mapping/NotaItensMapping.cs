using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using mongo_api.Models;
using mongo_api.Models.Cliente;

namespace mongo_api.Data.Mapping
{

    public class NotaItensMapping : IEntityTypeConfiguration<NotaItens>
    {
        public void Configure(EntityTypeBuilder<NotaItens> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Qtd)
                .HasColumnName("Qtd");

            builder.Property(x => x.Price)
                .HasColumnName("ValorUnitario")
                .HasPrecision(18,4)
                ;

            builder.HasOne(x => x.Produto)
                .WithMany(x => x.NotaItens)
                .HasForeignKey(x => x.ProdutoId);

            builder.HasOne(x => x.Nota)
               .WithMany(x => x.NotaItens)
               .HasForeignKey(x => x.NotaId);

            builder.ToTable("NotaItens");

        }
    }
}
