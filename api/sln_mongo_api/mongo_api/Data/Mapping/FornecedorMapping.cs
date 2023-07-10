using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using mongo_api.Models.Cliente;
using mongo_api.Models.Fornecedores;

namespace mongo_api.Data.Mapping
{
    public class FornecedorMapping : IEntityTypeConfiguration<Fornecedor>
    {
        public void Configure(EntityTypeBuilder<Fornecedor> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.CNPJ)
               .HasColumnName("CNPJ")
               .HasMaxLength(14);


            builder.Property(e => e.RazaoSocial)
               .HasColumnName("RazaoSocial")
               .HasMaxLength(200);

            builder.ToTable("Fornecedor");
        }
    }
}
