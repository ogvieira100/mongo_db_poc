
/*
 using System.Collections.Generic;
using System.Threading.Tasks;
using CursoMongo.Api.Data.Schemas;
using CursoMongo.Api.Domain.Entities;
using CursoMongo.Api.Domain.ValueObjects;
using MongoDB.Driver;
using System.Linq;
using CursoMongo.Api.Domain.Enums;
using MongoDB.Bson;
using MongoDB.Driver.Linq;

namespace CursoMongo.Api.Data.Repositories
{
    public class RestauranteRepository
    {
        IMongoCollection<RestauranteSchema> _restaurantes;
        IMongoCollection<AvaliacaoSchema> _avaliacoes;

        public RestauranteRepository(MongoDB mongoDB)
        {
            _restaurantes = mongoDB.DB.GetCollection<RestauranteSchema>("restaurantes");
            _avaliacoes = mongoDB.DB.GetCollection<AvaliacaoSchema>("avaliacoes");
        }

        public void Inserir(Restaurante restaurante)
        {
            var document = new RestauranteSchema
            {
                Nome = restaurante.Nome,
                Cozinha = restaurante.Cozinha,
                Endereco = new EnderecoSchema
                {
                    Logradouro = restaurante.Endereco.Logradouro,
                    Numero = restaurante.Endereco.Numero,
                    Cidade = restaurante.Endereco.Cidade,
                    Cep = restaurante.Endereco.Cep,
                    UF = restaurante.Endereco.UF
                }
            };

            _restaurantes.InsertOne(document);
        }

        public async Task<IEnumerable<Restaurante>> ObterTodos()
        {
            var restaurantes = new List<Restaurante>();

            await _restaurantes.AsQueryable().ForEachAsync(d =>
            {
                var r = new Restaurante(d.Id.ToString(), d.Nome, d.Cozinha);
                var e = new Endereco(d.Endereco.Logradouro, d.Endereco.Numero, d.Endereco.Cidade, d.Endereco.UF, d.Endereco.Cep);
                r.AtribuirEndereco(e);
                restaurantes.Add(r);
            });

            return restaurantes;
        }

        public Restaurante ObterPorId(string id)
        {
            var document = _restaurantes.AsQueryable().FirstOrDefault(_ => _.Id == id);

            if (document == null)
                return null;

            return document.ConverterParaDomain();
        }

        public bool AlterarCompleto(Restaurante restaurante)
        {
            var document = new RestauranteSchema
            {
                Id = restaurante.Id,
                Nome = restaurante.Nome,
                Cozinha = restaurante.Cozinha,
                Endereco = new EnderecoSchema
                {
                    Logradouro = restaurante.Endereco.Logradouro,
                    Numero = restaurante.Endereco.Numero,
                    Cidade = restaurante.Endereco.Cidade,
                    Cep = restaurante.Endereco.Cep,
                    UF = restaurante.Endereco.UF
                }
            };

            var resultado = _restaurantes.ReplaceOne(_ => _.Id == document.Id, document);

            return resultado.ModifiedCount > 0;
        }

        public bool AlterarCozinha(string id, ECozinha cozinha)
        {
            var atualizacao = Builders<RestauranteSchema>.Update.Set(_ => _.Cozinha, cozinha);

            var resultado = _restaurantes.UpdateOne(_ => _.Id == id, atualizacao);

            return resultado.ModifiedCount > 0;
        }

        public IEnumerable<Restaurante> ObterPorNome(string nome)
        {
            var restaurantes = new List<Restaurante>();

            _restaurantes.AsQueryable()
                .Where(_ => _.Nome.ToLower().Contains(nome.ToLower()))
                .ToList()
                .ForEach(d => restaurantes.Add(d.ConverterParaDomain()));

            return restaurantes;
        }

        public void Avaliar(string restauranteId, Avaliacao avaliacao)
        {
            var document = new AvaliacaoSchema
            {
                RestauranteId = restauranteId,
                Estrelas = avaliacao.Estrelas,
                Comentario = avaliacao.Comentario
            };

            _avaliacoes.InsertOne(document);
        }

        public async Task<Dictionary<Restaurante, double>> ObterTop3()
        {
            var retorno = new Dictionary<Restaurante, double>();

            var top3 = _avaliacoes.Aggregate()
                .Group(_ => _.RestauranteId, g => new { RestauranteId = g.Key, MediaEstrelas = g.Average(a => a.Estrelas)})
                .SortByDescending(_ => _.MediaEstrelas)
                .Limit(3);

            await top3.ForEachAsync(_ =>
            {
                var restaurante = ObterPorId(_.RestauranteId);
                
                _avaliacoes.AsQueryable()
                    .Where(a => a.RestauranteId == _.RestauranteId)
                    .ToList()
                    .ForEach(a => restaurante.InserirAvaliacao(a.ConverterParaDomain()));

                retorno.Add(restaurante, _.MediaEstrelas);
            });

            return retorno;
        }

        public async Task<Dictionary<Restaurante, double>> ObterTop3_ComLookup()
        {
            var retorno = new Dictionary<Restaurante, double>();

            var top3 = _avaliacoes.Aggregate()
                .Group(_ => _.RestauranteId, g => new { RestauranteId = g.Key, MediaEstrelas = g.Average(a => a.Estrelas)})
                .SortByDescending(_ => _.MediaEstrelas)
                .Limit(3)
                .Lookup<RestauranteSchema, RestauranteAvaliacaoSchema>("restaurantes", "RestauranteId", "Id", "Restaurante")
                .Lookup<AvaliacaoSchema, RestauranteAvaliacaoSchema>("avaliacoes", "Id", "RestauranteId", "Avaliacoes");

            await top3.ForEachAsync(_ =>
            {
                if (!_.Restaurante.Any())
                    return;

                var restaurante = new Restaurante(_.Id, _.Restaurante[0].Nome, _.Restaurante[0].Cozinha);
                var endereco = new Endereco(
                    _.Restaurante[0].Endereco.Logradouro,
                    _.Restaurante[0].Endereco.Numero,
                    _.Restaurante[0].Endereco.Cidade,
                    _.Restaurante[0].Endereco.UF,
                    _.Restaurante[0].Endereco.Cep);

                    restaurante.AtribuirEndereco(endereco);
                
                _.Avaliacoes.ForEach(a => restaurante.InserirAvaliacao(a.ConverterParaDomain()));

                retorno.Add(restaurante, _.MediaEstrelas);
            });

            return retorno;
        }

        public (long, long) Remover(string restauranteId)
        {
            var resultadoAvaliacoes = _avaliacoes.DeleteMany(_ => _.RestauranteId == restauranteId);
            var resultadoRestaurante = _restaurantes.DeleteOne(_ => _.Id == restauranteId);

            return (resultadoRestaurante.DeletedCount, resultadoAvaliacoes.DeletedCount);
        }

        public async Task<IEnumerable<Restaurante>> ObterPorBuscaTextual(string texto)
        {
            var restaurantes = new List<Restaurante>();

            var filter = Builders<RestauranteSchema>.Filter.Text(texto);

            await _restaurantes
                .AsQueryable()
                .Where(_ => filter.Inject())
                .ForEachAsync(d => restaurantes.Add(d.ConverterParaDomain()));

            return restaurantes;
        }
    }
}
 
 */
using Microsoft.AspNetCore.Mvc.RazorPages;
using mongo_api.Data.Context;
using mongo_api.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System.Linq.Expressions;

namespace mongo_api.Data.Repository
{
    public  class BaseConsultRepositoryMongo<TEntity>
           : IBaseConsultRepositoryMongo<TEntity> where TEntity : BaseMongo
    {


        readonly MongoContext _mongoContext;
        

        public IMongoCollection<TEntity> MongoCollectionConsult { get; }

        public BaseConsultRepositoryMongo(MongoContext mongoContext)
        {
            _mongoContext = mongoContext;
            MongoCollectionConsult = _mongoContext.DB
                            .GetCollection<TEntity>(((TEntity)Activator.CreateInstance(typeof(TEntity))).TableName);
        }
        public void Dispose() => GC.SuppressFinalize(this);

        public async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate)
        =>  await MongoCollectionConsult.AsQueryable().Where(predicate).AnyAsync(); 
        

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        => await (await MongoCollectionConsult.FindAsync(new BsonDocument())).ToListAsync();

        public async Task<TEntity> GetByIdAsync(string id)
        => await (await MongoCollectionConsult.FindAsync(x => x.RelationalId == id)).FirstOrDefaultAsync();

        public async Task<IEnumerable<TEntity>> SearchAsync(Expression<Func<TEntity, bool>> predicate)
        => await (await MongoCollectionConsult.FindAsync(predicate)).ToListAsync();

        public async Task<PagedDataResponse<TEntity>> PaginateAsync(PagedDataRequest pagedDataRequest,
                                                                    Expression<Func<TEntity, bool>>? predicate)
        {
            if (predicate is null)
            return await MongoCollectionConsult.Find(new BsonDocument()).PaginateAsync(pagedDataRequest, null);

            return await MongoCollectionConsult.Find(predicate).PaginateAsync(pagedDataRequest, null);

        } 

        public async Task<IEnumerable<TEntity>> SearchTextAsync(string text)
        {
           var  vvv =  MongoCollectionConsult.Find(new BsonDocument());

            var lists = new List<TEntity>();

            var filter = Builders<TEntity>.Filter.Text(text);

            await MongoCollectionConsult
                .AsQueryable()
                .Where(_ => filter.Inject())
                .ForEachAsync(d => lists.Add(d));

            return lists;
        }
    }
}
