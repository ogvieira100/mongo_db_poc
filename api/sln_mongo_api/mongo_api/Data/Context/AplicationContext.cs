using Microsoft.EntityFrameworkCore;
using mongo_api.Data.Mapping;
using mongo_api.Data.Repository;
using mongo_api.Models.Cliente;
using System.Reflection.Emit;

namespace mongo_api.Data.Context
{

    public class AplicationContext : DbContext
    {

        readonly IClientesMongoManage _clientesMongoManage;
        readonly IEnderecoMongoMange _enderecoMongoMange;
        public AplicationContext(DbContextOptions<AplicationContext> options,
            IClientesMongoManage clientesMongoManage,

            IEnderecoMongoMange enderecoMongoMange)
         : base(options)
        {

            _clientesMongoManage = clientesMongoManage;
            _enderecoMongoMange = enderecoMongoMange;
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var ret = await base.SaveChangesAsync(cancellationToken);
            await SaveChangesMongoAsync(ret);
            return ret;
        }

        private async Task SaveChangesMongoAsync(int ret)
        {
            List<Tuple<EntityState, Type, object>> entrys = new List<Tuple<EntityState, Type, object>>();
            foreach (var entry in ChangeTracker.Entries())
            {
                var baseEntry = entry.Entity;
                entrys.Add(new Tuple<EntityState, Type, object>(entry.State,
                                                                baseEntry.GetType(),
                                                                baseEntry));
            }
            if (ret > 0)
            {

                #region " Clientes "
                var tupleClientes = entrys.Where(x => x.Item2 == typeof(Clientes))
                               .Select(x => new Tuple<EntityState, Clientes>(x.Item1, (x.Item3 as Clientes)))
                               .ToList();

                await _clientesMongoManage.ExecManager(tupleClientes);

                #endregion

                #region " Enderecos "
                var tupleEnderecos = entrys.Where(x => x.Item2 == typeof(Endereco))
                                     .Select(x => new Tuple<EntityState, Endereco>(x.Item1, (x.Item3 as Endereco)))
                                     .ToList();

                await _enderecoMongoMange.ExecManager(tupleEnderecos);
                #endregion

            }
        }

        public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess,
            CancellationToken cancellationToken = default)
        {
            var ret = await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
            await SaveChangesMongoAsync(ret);
            return ret;

        }

        public override int SaveChanges()
        {
            return base.SaveChanges();
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new EnderecoMapping());
            modelBuilder.ApplyConfiguration(new ClientesMapping());
            modelBuilder.ApplyConfiguration(new FornecedorMapping());
            modelBuilder.ApplyConfiguration(new NotaMapping());
            modelBuilder.ApplyConfiguration(new NotaItensMapping());
            modelBuilder.ApplyConfiguration(new PedidosItensMapping());
            modelBuilder.ApplyConfiguration(new PedidosMapping());
            modelBuilder.ApplyConfiguration(new ProdutosMapping());



            //FornecedorMapping
        }
    }
}
