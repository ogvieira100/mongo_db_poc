using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using mongo_api.Models.Cliente;

namespace mongo_api.Data.Mapping
{
    public class ClientesMapping : IEntityTypeConfiguration<Clientes>
    {
        public void Configure(EntityTypeBuilder<Clientes> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.CPF)
                .HasColumnName("CPF")
                .HasMaxLength(11);


            builder.Property(e => e.Nome)
               .HasColumnName("Nome")
               .HasMaxLength(200);
        }
    }
}
