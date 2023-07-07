using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using mongo_api.Models;

namespace mongo_api.Data.Mapping
{
    public class NotaMapping : IEntityTypeConfiguration<Nota>
    {
        public void Configure(EntityTypeBuilder<Nota> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Numero)
            .HasColumnName("Numero")
            .HasMaxLength(50);

            builder.Property(x => x.Observation)
           .HasColumnName("Observacao")
           .HasMaxLength(int.MaxValue);

            builder.HasOne(x => x.Cliente)
                .WithMany(x => x.Notas)
                .HasForeignKey(x => x.ClienteId);

            builder.HasOne(x => x.Fornecedor)
                .WithMany(x => x.Notas)
                .HasForeignKey(x => x.FornecedorId);

            builder.ToTable("Nota");

        }
    }
}
