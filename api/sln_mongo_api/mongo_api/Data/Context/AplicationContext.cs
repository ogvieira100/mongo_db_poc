using MediatR;
using Microsoft.EntityFrameworkCore;
using mongo_api.Data.Mapping;
using mongo_api.Data.Repository;
using mongo_api.Models.Cliente;
using mongo_api.Models.Fornecedores;
using mongo_api.Models.Notas;
using mongo_api.Models.Pedidos;
using mongo_api.Models.Produto;
using mongo_api.Util;
using System.Linq;
using System.Reflection.Emit;

namespace mongo_api.Data.Context
{

    public class AplicationContext : DbContext
    {

        readonly IClientesMongoManage _clientesMongoManage;
        readonly IEnderecoMongoMange _enderecoMongoMange;
        readonly IProdutoMongoManage _produtoMongoManage;
        readonly IFornecedorMongoManage _fornecedorMongoManage;
        readonly IPedidoMongoManage _pedidoMongoManage;
        readonly INotaMongoManage _notaMongoManage;
        readonly IPedidoItensMongoManage _pedidoItensMongoManage;
        readonly IMediator _mediator;
        public AplicationContext(DbContextOptions<AplicationContext> options,
            IClientesMongoManage clientesMongoManage,
            IProdutoMongoManage produtoMongoManage,
            IFornecedorMongoManage fornecedorMongoManage,
            IPedidoMongoManage pedidoMongoManage,
            INotaMongoManage notaMongoManage,
            IMediator mediator, 
            IPedidoItensMongoManage pedidoItensMongoManage, 
            IEnderecoMongoMange enderecoMongoMange)
         : base(options)
        {
            _produtoMongoManage = produtoMongoManage;
            _clientesMongoManage = clientesMongoManage;
            _enderecoMongoMange = enderecoMongoMange;
            _fornecedorMongoManage = fornecedorMongoManage;
            _pedidoMongoManage = pedidoMongoManage;
            _notaMongoManage = notaMongoManage;
            _mediator = mediator;
            _pedidoItensMongoManage = pedidoItensMongoManage;   
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entrys = GetEntrys();
            var ret = await base.SaveChangesAsync(cancellationToken);
            return ret;
        }

        List<Tuple<EntityState, Type, object>> GetEntrys()
        {
            List<Tuple<EntityState, Type, object>> entrys = new List<Tuple<EntityState, Type, object>>();
            foreach (var entry in ChangeTracker.Entries())
            {
                var baseEntry = entry.Entity;
                entrys.Add(new Tuple<EntityState, Type, object>(entry.State,
                                                                baseEntry.GetType(),
                                                                baseEntry));
            }
            return entrys;
        }

        private async Task SaveChangesMongoAsync(int ret, List<Tuple<EntityState, Type, object>> entrys)
        {
           
            if (ret > 0)
            {
                #region " Pedidos "

                var tuplePedidos = entrys.Where(x => x.Item2 == typeof(Pedido))
                              .Select(x => new Tuple<EntityState, Pedido>(x.Item1, (x.Item3 as Pedido)))
                              .ToList();

                await _pedidoMongoManage.ExecManager(tuplePedidos);

                #endregion

                #region " Notas "

                var tupleNotas = entrys.Where(x => x.Item2 == typeof(Nota))
                           .Select(x => new Tuple<EntityState, Nota>(x.Item1, (x.Item3 as Nota)))
                           .ToList();

                await _notaMongoManage.ExecManager(tupleNotas);

                #endregion

                #region " Pedido Itens "

                var tuplePedidosItens = entrys.Where(x => x.Item2 == typeof(PedidoItens))
                           .Select(x => new Tuple<EntityState, PedidoItens>(x.Item1, (x.Item3 as PedidoItens)))
                           .ToList();

                await _pedidoItensMongoManage.ExecManager(tuplePedidosItens);

                #endregion

                #region " Produtos "

                var tupleProdutos = entrys.Where(x => x.Item2 == typeof(Produtos))
                               .Select(x => new Tuple<EntityState, Produtos>(x.Item1, (x.Item3 as Produtos)))
                               .ToList();

                 await _produtoMongoManage.ExecManager(tupleProdutos);


                #endregion

                #region " Clientes "
                var tupleClientes = entrys.Where(x => x.Item2 == typeof(Clientes))
                               .Select(x => new Tuple<EntityState, Clientes>(x.Item1, (x.Item3 as Clientes)))
                               .ToList();

                await _clientesMongoManage.ExecManager(tupleClientes);

                #endregion

                #region " Enderecos "
                var tupleEnderecos = entrys.Where(x => x.Item2 == typeof(Endereco))
                                     .Select(x => new Tuple<EntityState,Endereco>(x.Item1, (x.Item3 as Endereco)))
                                     .ToList();

                await _enderecoMongoMange.ExecManager(tupleEnderecos);
                #endregion

                #region " Fornecedor "

                var tupleFornecedores = entrys.Where(x => x.Item2 == typeof(Fornecedor))
                             .Select(x => new Tuple<EntityState, Fornecedor>(x.Item1, (x.Item3 as Fornecedor)))
                             .ToList();

                 await _fornecedorMongoManage.ExecManager(tupleFornecedores);
                #endregion

                await _mediator.PublicarEventos(this);
            }
        }

        public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess,
            CancellationToken cancellationToken = default)
        {
            var entrys = GetEntrys();
            var ret = await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
            await SaveChangesMongoAsync(ret, entrys);
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
        }
    }
}
