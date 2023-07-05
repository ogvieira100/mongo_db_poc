using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using mongo_api.Models.Cliente;

namespace mongo_api.Data.Mapping
{
    public class EnderecoMapping : IEntityTypeConfiguration<Endereco>
    {
        public void Configure(EntityTypeBuilder<Endereco> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Estado)
                .HasColumnName("Estado")
                ;

            builder.Property(e => e.Logradouro)
               .HasColumnName("Logradouro")
               .HasMaxLength(200)
               ;

            builder.Property(e => e.ClienteId)
               .HasColumnName("ClienteId")
               ;

            builder.HasOne(x => x.Cliente)
                  .WithMany(x => x.Enderecos)
                  .HasForeignKey(x => x.ClienteId);

            builder.ToTable("Endereco");
                  
        }
    }
}
