using Accounts.API.AutofacModules;
using Accounts.API.Extensions;
using Autofac.Extensions.DependencyInjection;
using Autofac;
using Microsoft.OpenApi.Models;
using Accounts.Domain.Settings;
using StockAPI.Domain.Abstraction.DataBase;
using StockAPI.Domain.Services.AppSettings;
using StockAPI.Domain.Services;

var builder = WebApplication.CreateBuilder(args);

var sqliteConnection = builder.Configuration.GetConnectionString("SQLiteConnection");

builder.Services.AddSingleton<IDataBaseContext>(provider => new DataBaseContext(sqliteConnection));

builder.Host
    .UseServiceProviderFactory(new AutofacServiceProviderFactory())
    .ConfigureContainer<ContainerBuilder>(autofacBuilder =>
    {
        autofacBuilder.RegisterModule<ServicesModule>();
        autofacBuilder.RegisterModule<RepositoriesModule>();
        autofacBuilder.RegisterModule<ProvidersModule>();
        autofacBuilder.RegisterModule<FactoriesModule>();
        autofacBuilder.RegisterModule<ClientsModule>();
    });

builder.Services.AddControllers();

builder.AddJwtAuthentication();

builder.Services.AddEndpointsApiExplorer()
                .AddHttpContextAccessor()
                .AddPolicyBasedRoleAuthorizationServices()
                .Configure<HostsSettings>(builder.Configuration.GetSection("HostsSettings"))
                .Configure<MongoDBSettings>(builder.Configuration.GetSection("MongoDB"))
                .Configure<ApiKeys>(builder.Configuration.GetSection("ApiKeys"))
                .Configure<EndPoints>(builder.Configuration.GetSection("EndPoints"));

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Accounts API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Example: \"Athorization: Bearer {token}\"",
        Type = SecuritySchemeType.ApiKey,
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Name = "Authorization"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

builder.Services.AddStocksAutomapper();

var app = builder.Build();

app.ConfigureSafelistMiddleware(builder.Configuration["AdminSafeList"]);

app.ConfigureLogMiddleware();

app.ConfigureCustomExceptionMiddleware();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();