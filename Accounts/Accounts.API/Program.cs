using Accounts.API.AutofacModules;
using Accounts.API.Extensions;
using Autofac.Extensions.DependencyInjection;
using Autofac;
using Microsoft.OpenApi.Models;
using Accounts.Domain.Settings;

var builder = WebApplication.CreateBuilder(args);

builder.Host
    .UseServiceProviderFactory(new AutofacServiceProviderFactory())
    .ConfigureContainer<ContainerBuilder>(autofacBuilder =>
    {
        autofacBuilder.RegisterModule<ServicesModule>();
        autofacBuilder.RegisterModule<RepositoriesModule>();
        autofacBuilder.RegisterModule<ProvidersModule>();
    });

builder.Services.AddControllers();

builder.AddJwtAuthentication();

builder.Services.AddAuthorization();

builder.Services.AddEndpointsApiExplorer()
                .AddHttpContextAccessor()
                .Configure<HostsSettings>(builder.Configuration.GetSection("HostsSettings"));

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "StockManagement API", Version = "v1" });
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

app.ConfigureCustomExceptionMiddleware();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
