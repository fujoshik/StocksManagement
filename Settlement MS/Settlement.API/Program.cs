using Quartz;
using Settlement.Domain;
using Settlement.Domain.Abstraction.Repository;
using Settlement.Domain.Abstraction.Routes;
using Settlement.Domain.Abstraction.Services;
using Settlement.Domain.Constants;
using Settlement.Domain.Services;
using Settlement.Infrastructure.Repository;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<ISettlementService, SettlementService>();
builder.Services.AddScoped<IHttpClientService, ConnectionService>();
builder.Services.AddScoped<IWalletRoutes, WalletRoutes>();
builder.Services.AddScoped<IStockRoutes, StockRoutes>();
builder.Services.AddScoped<ISettlementRepository, SettlementRepository>();

builder.Services.AddQuartz(q =>
{
    var dailySettlementJob = JobKey.Create(nameof(DailySettlementJob));
    q.AddJob<DailySettlementJob>(dailySettlementJob)
        .AddTrigger(t => t
            .ForJob(dailySettlementJob)
            .WithCronSchedule(CronExpressionConstant.cronExpression));

    var processFailedTransactions = JobKey.Create(nameof(ProcessFailedTransactionsJob));
    q.AddJob<ProcessFailedTransactionsJob>(processFailedTransactions)
        .AddTrigger(t => t
            .ForJob(processFailedTransactions)
            .WithCronSchedule(CronExpressionConstant.cronExpression));
});

builder.Services.AddQuartzHostedService();

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
