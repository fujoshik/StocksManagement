using Microsoft.Data.Sqlite;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using StockAPI.Domain.Abstraction.DataBase;
using StockAPI.Domain.Abstraction.Mappers;
using StockAPI.Domain.Abstraction.Services;
using StockAPI.Domain.Services;
using StockAPI.Domain.Services.AppSettings;
using StockAPI.Domain.Services.Mappers;
using System.Configuration;

SQLitePCL.Batteries.Init();

var builder = WebApplication.CreateBuilder(args);

//load configuration from appsettings.json
var configuration = new ConfigurationBuilder()
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json")
    .Build();

//sqlite 
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

//DataBaseContext
builder.Services.AddSingleton<IDataBaseContext>(provider => new DataBaseContext(connectionString));

builder.Services.Configure<ApiKeys>(configuration.GetSection("ApiKeys"));
builder.Services.Configure<EndPoints>(configuration.GetSection("EndPoints"));

//logger
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration).CreateLogger();

builder.Host.UseSerilog();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//adding my service
builder.Services.AddScoped<IStockAPIService, StockAPIService>();

builder.Services.AddScoped<IStockMapper, StockMapper>();

builder.Services.AddHttpClient();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

