using Analyzer.API.Analyzer.Domain.Abstracions.Interfaces;
using Analyzer.API.Analyzer.Domain.Abstracions.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Cors;
using Microsoft.Extensions.DependencyInjection;
using Analyzer.API;
using Analyzer.API.Analyzer.Domain.Services;
using Analyzer.API.Analyzer.Domain;
using Analyzer.API.Analyzer.Domain.DTOs;

var builder = WebApplication.CreateBuilder(args);
var configuration = new ConfigurationBuilder()
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json")
    .Build();

// Add services to the container.
builder.Services.AddControllers();  // Add this line to add controllers' services

// Load CORS configuration from appsettings.json
var corsConfig = configuration.GetSection("CORSConfig").Get<CORSConfig>();

// Enable CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigins", builder =>
    {
        builder.WithOrigins(corsConfig.AllowedOrigins)
               .AllowAnyHeader()
               .AllowAnyMethod();
    });
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<ClosePriceOpenPrice>();
builder.Services.AddScoped<PercentageChangeCalculator>();
builder.Services.AddScoped<IService, ApiService>();
builder.Services.AddScoped<ICalculationService, CalculationService>();
builder.Services.AddScoped<IHttpClientService, HttpClientService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Use CORS policy
app.UseCors("AllowSpecificOrigins");

app.UseAuthorization();

app.MapControllers();

app.Run();
