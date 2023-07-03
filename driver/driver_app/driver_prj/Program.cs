// See https://aka.ms/new-console-template for more information
using MongoDB.Driver.Core.Configuration;

using MongoDB.Driver;
using MongoDB.Bson;
using driver_prj;
using MongoDB.Bson.Serialization;
using System;

Console.WriteLine("Mongo Db Connect");


// Replace the following with your MongoDB deployment's connection string.
string _mongoConnectionString = "mongodb+srv://app:app@cluster0.sy4e2a7.mongodb.net/";


var client = new MongoClient(_mongoConnectionString);

var database = client.GetDatabase("sample_mflix");

var collection = database.GetCollection<BsonDocument>("books");

var collectionTyped = database.GetCollection<Books>("books");



var myFilter = Builders<BsonDocument>.Filter;

//Somando dois filters
var filterConditions = myFilter.Gt("name", "A") & myFilter.Lte("name", "B");

/*
 * como fazer atualizações crie o filter crie o updata com o campo e valor e na collection atualize
var update = Builders<BsonDocument>.Update.Set("name","livro dos anjos");

await collection.UpdateOneAsync(filterConditions, update);

await collection.DeleteOneAsync(filterConditions); -> pra deletar passe o filter
*/


/*
 * 
 * https://www.mongodb.com/docs/drivers/csharp/current/fundamentals/class-mapping/
 
forma de usar o fluente

BsonClassMap.RegisterClassMap<Books>(classMap =>
{
    classMap.AutoMap();
    classMap.MapIdMember(x => x.Id);->ID
    classMap.MapMember(p => p.Name).SetElementName("name");
    classMap.MapMember(p => p.Name).SetIgnoreIfDefault(true);
    classMap.MapMember(p => p.Name).SetDefaultValue("trutru");
    classMap.UnmapMember(p => p.Name);
    classMap.SetIgnoreExtraElements(true);
});

BsonClassMap.RegisterClassMap<Person>(classMap =>
{
    classMap.MapMember(p => p.Name);
    classMap.MapMember(p => p.Age);
    classMap.MapMember(p => p.Hobbies);
});

BsonClassMap.RegisterClassMap<Person>(classMap =>
{
    classMap.AutoMap();
    classMap.MapMember(p => p.Hobbies).SetElementName("favorite_hobbies");
});
 
 */


var filter = myFilter.Eq("name", "volta ao mundo em 180 dias");

var filterBooks = myFilter.Eq("name", "volta ao mundo em 180 dias");

var firstCollection = await collection.Find(new BsonDocument()).FirstOrDefaultAsync();

var firstCollectionTyped = await collectionTyped.Find(x=>x.Name == "volta ao mundo em 180 dias").FirstOrDefaultAsync();

/*
var query = collectionTyped.AsQueryable();

var queryBooks = query.AsQueryable();
*/
    
/*
 
    é possivel trabalhar com expressoes 
 
 */


await collection.Find(new BsonDocument()).ForEachAsync(x => Console.WriteLine(x));



var document = collection.Find(filter).First();
var documentBooks = collection.Find(filter).First();

Console.WriteLine("Document");
Console.WriteLine(document);

Console.WriteLine("FirstCollection");
Console.WriteLine(firstCollection);

Console.WriteLine("Document Books");
Console.WriteLine(documentBooks);

var filterConplex = Builders<Books>.Filter
    .Eq(r => r.Writer, "José vinicius");

var findComplex =  await collection.Find(filter).ToListAsync();


Console.WriteLine("Document Complex");
Console.WriteLine(findComplex.FirstOrDefault());


string[] books = { "" };

string[] writers = { "" };

var booksAddBson = new BsonDocument {
        
            {"name","" },
            {"writer","" }
    
};

var bookClass = new Books { 



};






Console.ReadKey();