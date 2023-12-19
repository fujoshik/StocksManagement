using Microsoft.OpenApi.Models;
using Analyzer.Domain.Abstracions.Interfaces;
using Analyzer.API.Analyzer.Domain.Abstracions.Services;
using Analyzer.API.Analyzer.Domain.Services;




var builder = WebApplication.CreateBuilder(args);
var configuration = new ConfigurationBuilder()
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json")
    .Build();

builder.Services.AddControllers();
builder.Services.AddHttpClient("StockAPI"); 


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Your API Name", Version = "v1" });
});


builder.Services.AddScoped<IPercentageChange, PercentageChangeService>();
builder.Services.AddScoped<IDailyYieldChanges, DailyYieldChangesService>();
builder.Services.AddScoped<ICalculationService, CalculateCurrentYieldService>();
builder.Services.AddScoped<IHttpClientService, HttpClientService>();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Your API Name v1"));
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
