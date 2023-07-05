using MediatR;
using Microsoft.EntityFrameworkCore;
using mongo_api.Data.Context;
using mongo_api.Data.Repository;
using mongo_api.Models.Cliente;
using MongoDB.Driver.Core.Operations;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

ConfigurationManager configuration = builder.Configuration; // allows both to access and to set up the config
IWebHostEnvironment environment = builder.Environment;

builder.Configuration.AddJsonFile("appsettings.json", true, true)
                    .SetBasePath(environment.ContentRootPath)
                    .AddJsonFile($"appsettings.{environment.EnvironmentName}.json", true, true)
                    .AddEnvironmentVariables();
;

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddMediatR(Assembly.GetExecutingAssembly());
builder.Services.AddControllers()
             .AddJsonOptions(options =>
             {
                 // options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                 // options.JsonSerializerOptions.IgnoreNullValues = true;
             });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{

    options.AddPolicy("Development",
          builder =>
              builder
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowAnyOrigin()
              ); // allow credentials

    options.AddPolicy("Production",
        builder =>
            builder
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowAnyOrigin()
              ); // allow credentials
});

#region  " Sql "

var connectionString = configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AplicationContext>(options =>
     options.UseSqlServer(connectionString)
     .EnableSensitiveDataLogging()
     .UseLazyLoadingProxies()
     );


#endregion


//

builder.Services.AddScoped<AplicationContext>();
builder.Services.AddScoped<MongoContext>();


builder.Services.AddScoped<IClienteQuery, ClienteQuery>();
builder.Services.AddScoped<IClienteMongoRepository, ClienteMongoRepository>();
builder.Services.AddScoped<IClientesMongoManage, ClientesMongoManage>();
//IClientesMongoManage

//MongoContext

//

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped(typeof(IBaseRepositoryMongo<>), typeof(BaseRepositoryMongo<>));
builder.Services.AddScoped(typeof(IBaseConsultRepositoryMongo<>), typeof(BaseConsultRepositoryMongo<>));

builder.Services.AddScoped(typeof(IBaseConsultRepository<>), typeof(BaseConsultRepository<>));
builder.Services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
//

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
    app.UseCors("Development");
}
else
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
    app.UseCors("Development");

}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();



app.UseStaticFiles();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});


app.MigrateDatabase();



app.Run();

