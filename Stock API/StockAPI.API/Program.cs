using AutoMapper;
using FluentScheduler;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using StockAPI.API.Middleware;
using StockAPI.Domain.Abstraction.DataBase;
using StockAPI.Domain.Abstraction.Mappers;
using StockAPI.Domain.Abstraction.Services;
using StockAPI.Domain.Services;
using StockAPI.Domain.Services.AppSettings;
using StockAPI.Domain.Services.Mappers;
using StockAPI.Domain.Services.Scheduling;
using System;
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

//using automapper
var mappingConfig = new MapperConfiguration(mc =>
{
    mc.AddProfile(new MappingProfile()); // Add your mapping profile
});

IMapper mapper = mappingConfig.CreateMapper();
builder.Services.AddSingleton(mapper);


builder.Services.AddHttpClient();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//ip address safe list 
app.UseMiddleware<SafelistMiddleware>(builder.Configuration["AdminSafeList"]);

app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

//fluentscheduler
using (var scope = app.Services.CreateScope())
{
    var stockApiService = scope.ServiceProvider.GetRequiredService<IStockAPIService>();
    JobManager.Initialize(new MyRegistry(stockApiService));
}

app.Run();

