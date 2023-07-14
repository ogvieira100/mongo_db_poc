using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using mongo_api.Data.Context;
using mongo_api.Models;
using mongo_api.Models.Cliente;
using mongo_api.Models.Fornecedores;
using mongo_api.Models.Pedidos;
using mongo_api.Models.Produto;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Linq.Expressions;

namespace mongo_api.Data.Repository
{

    public interface IPedidoMongoRepository: IBaseRepositoryMongo<PedidoMongo>
    {
        Task<PedidoMongo> GetPedidoByRelationalId(string relationalId);

        Task<PedidoMongo> GetPedidoUpdateByRelationalId(string relationalId);

        Task UpdatePedidoMongo(PedidoMongo x);

        Task<PagedDataResponse<PedidoMongo>> PagedPedidos(PedidoPagedRequest clientePagedRequest);
    }
    public class PedidoMongoRepository : BaseRepositoryMongo<PedidoMongo>, IPedidoMongoRepository
    {

        readonly IMongoCollection<ClientesMongo>   _clienteMongoCollection;
        readonly IMongoCollection<FornecedorMongo> _fornecedorMongoCollection;
        readonly IMongoCollection<PedidoItensMongo> _pedidoItensMongoCollection;
        readonly IMongoCollection<ProdutoMongo> _produtosMongoCollection;
        public PedidoMongoRepository(MongoContext mongoContext,
                                     
                                     IBaseConsultRepositoryMongo<PedidoMongo> baseConsultRepositoryMongo) 
            : base(mongoContext, baseConsultRepositoryMongo)
        {
             _clienteMongoCollection = mongoContext.DB.GetCollection<ClientesMongo>( new ClientesMongo().TableName);
            _fornecedorMongoCollection = mongoContext.DB.GetCollection<FornecedorMongo>(new FornecedorMongo().TableName);
            _pedidoItensMongoCollection = mongoContext.DB.GetCollection<PedidoItensMongo>(new PedidoItensMongo().TableName);
            _produtosMongoCollection = mongoContext.DB.GetCollection<ProdutoMongo>(new ProdutoMongo().TableName);
        }

        public async Task<PedidoMongo> GetPedidoByRelationalId(string relationalId)
        {
            var lstPedidoMongo = new List<PedidoMongo>();
            var col = (await MongoCollectionPersist.FindAsync(x => x.RelationalId == relationalId));
            /*informações que podem ter sido atualizadas posteriormente*/
            await GetPedidos(lstPedidoMongo, col);

            return lstPedidoMongo.FirstOrDefault() ?? new PedidoMongo();
        }

        async Task GetPedidos(List<PedidoMongo> lstPedidoMongo,
                 IFindFluent<PedidoMongo,PedidoMongo> query)
        {
            await query.ForEachAsync(async x =>
            {
                await UpdateFinalyListMongo(lstPedidoMongo, x);

            });
        }

        async Task UpdateFinalyListMongo(List<PedidoMongo> lstPedidoMongo, PedidoMongo x)
        {
            await UpdatePedidoMongo(x);
            lstPedidoMongo.Add(x);
        }

        public async Task UpdatePedidoMongo(PedidoMongo x)
        {
            var cliCol = (await _clienteMongoCollection.FindAsync(cli => cli.RelationalId == x.ClienteId))?.FirstOrDefault();
            var fornCol = (await _fornecedorMongoCollection.FindAsync(forn => forn.RelationalId == x.FornecedorId))?.FirstOrDefault();
            if (cliCol is not null)
                x.Cliente = cliCol;
            if (fornCol is not null)
                x.Fornecedor = fornCol;
            var produtos = (await _produtosMongoCollection
                                 .FindAsync(prod =>
                                 x.PedidoItens
                                 .Select(pedit => pedit.ProdutoId)
                                 .Contains(prod.RelationalId))).ToList();
            foreach (var item in x.PedidoItens)
            {
                var prodSearch = produtos?.FirstOrDefault(x => x.RelationalId == item.ProdutoId);
                if (prodSearch is not null)
                    item.Produto = prodSearch;
            }
        }

        async Task GetPedidos(List<PedidoMongo> lstPedidoMongo, 
                   IAsyncCursor<PedidoMongo> col)
        {
            await col.ForEachAsync(async x =>
            {
                await UpdateFinalyListMongo(lstPedidoMongo, x);
            });
        }

        public async Task<PagedDataResponse<PedidoMongo>> PagedPedidos(PedidoPagedRequest pagedDataRequest)
        {
            var lstPedidoMongo = new List<PedidoMongo>();
            var paged = new PagedDataResponse<PedidoMongo>();


            var query = ( MongoCollectionPersist.Find(new BsonDocument()));

            pagedDataRequest.Page = (pagedDataRequest.Page < 0) ? 1 : pagedDataRequest.Page;

            paged.Page = pagedDataRequest.Page;
            paged.PageSize = pagedDataRequest.Limit;

            long totalItemsCountTask = 0;


            totalItemsCountTask = await query.CountDocumentsAsync();

            var startRow = (pagedDataRequest.Page - 1) * pagedDataRequest.Limit;
            if (startRow > 0)
                query = query.Skip(startRow).Limit(paged.PageSize); ;


            await GetPedidos(lstPedidoMongo, query);

            paged.Items = lstPedidoMongo;


            paged.TotalItens = totalItemsCountTask;
            paged.TotalPages = (int)Math.Ceiling(paged.TotalItens / (double)pagedDataRequest.Limit);

            return paged;
           

        }

        public async Task<PedidoMongo> GetPedidoUpdateByRelationalId(string relationalId)
        => await ( MongoCollectionPersist.Find(x => x.RelationalId == relationalId)).FirstOrDefaultAsync(); 
        
    }
}
