using System;
using System.Net.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Analyzer.Domain.Abstracions.Interfaces;
using Analyzer.API.Analyzer.Domain.Abstracions.Services;
using Analyzer.API.Analyzer.Domain.Services;
using Analyzer.API.Analyzer.Domain.DTOs;
using Analyze.Domain.Service;
using StockAPI.Infrastructure;


var builder = WebApplication.CreateBuilder(args);
var configuration = new ConfigurationBuilder()
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json")
    .Build();

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddHttpClient("StockAPI"); // Add a name for the HttpClient registration

// Add CORS configuration if needed
// builder.Services.AddCors(options =>
// {
//     options.AddPolicy("AllowSpecificOrigins", builder =>
//     {
//         builder.WithOrigins("https://example.com")
//                .AllowAnyHeader()
//                .AllowAnyMethod();
//     });
// });

// Configure Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Your API Name", Version = "v1" });
});

// Register your services
builder.Services.AddScoped<IPercentageChange,PercentageChangeService >();
builder.Services.AddScoped<IDailyYieldChanges, DailyYieldChangesService>();
builder.Services.AddScoped<IService, ApiService>();
builder.Services.AddScoped<ICalculationService, CalculationService>();
builder.Services.AddScoped<IHttpClientService, HttpClientService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Your API Name v1"));
}

app.UseHttpsRedirection();

// Enable CORS if needed
// app.UseCors("AllowSpecificOrigins");

app.UseAuthorization();

app.MapControllers();

app.Run();
