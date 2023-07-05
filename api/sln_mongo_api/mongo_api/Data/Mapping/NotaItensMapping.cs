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
            throw new NotImplementedException();
        }
    }
}
