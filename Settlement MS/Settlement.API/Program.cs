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
    var job = JobKey.Create(nameof(DailySettlementJob));
    q.AddJob<DailySettlementJob>(job)
            .AddTrigger(t => t
            .ForJob(job)//.WithCronSchedule(CronExpressionConstant.cronExpression));
            .WithDailyTimeIntervalSchedule(x => x
                .WithIntervalInMinutes(5)));
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
