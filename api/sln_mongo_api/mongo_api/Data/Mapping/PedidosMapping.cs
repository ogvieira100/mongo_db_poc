using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using mongo_api.Models.Pedidos;

namespace mongo_api.Data.Mapping
{
    public class PedidosMapping : IEntityTypeConfiguration<Pedido>
    {
        public void Configure(EntityTypeBuilder<Pedido> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Observation)
                   .HasColumnName("Observacao")
                   .HasMaxLength(int.MaxValue)
                   .IsRequired(false)
                   ;

            builder.HasOne(x => x.Cliente)
                .WithMany(x => x.Pedidos)
                .HasForeignKey(x => x.ClienteId);

            builder.HasOne(x => x.Fornecedor)
               .WithMany(x => x.Pedidos)
               .HasForeignKey(x => x.FornecedorId);

            builder.ToTable("Pedido");
        }
    }
}
