using Settlement.Domain.Abstraction.Services;
using Settlement.Domain.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<ISettlementService, SettlementService>();
builder.Services.AddScoped<IHttpClientService, HttpClientService>();
builder.Services.AddScoped<IWalletRoutes, WalletRoutes>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient();
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
