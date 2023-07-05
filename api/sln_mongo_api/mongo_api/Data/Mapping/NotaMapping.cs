using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using mongo_api.Models;

namespace mongo_api.Data.Mapping
{
    public class NotaMapping : IEntityTypeConfiguration<Nota>
    {
        public void Configure(EntityTypeBuilder<Nota> builder)
        {
            throw new NotImplementedException();
        }
    }
}
