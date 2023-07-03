using Microsoft.EntityFrameworkCore;
using mongo_api.Data.Mapping;
using mongo_api.Data.Repository;
using System.Reflection.Emit;

namespace mongo_api.Data.Context
{
    public class AplicationContext : DbContext
    {

        public AplicationContext(DbContextOptions<AplicationContext> options)
         : base(options)
        {



        }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new EnderecoMapping());
            modelBuilder.ApplyConfiguration(new ClientesMapping());
            //
        }
    }
}
