using API.Gateway.Middleware;
using Gateway.Domain.Abstraction.Clients;
using Gateway.Domain.Abstraction.Clients.AccountClient;
using Gateway.Domain.Abstraction.Factories;
using Gateway.Domain.Abstraction.Services;
using Gateway.Domain.Clients;
using Gateway.Domain.Clients.AccountClient;
using Gateway.Domain.Factories;
using Gateway.Domain.Services;
using Gateway.Domain.Settings;
using Gateway.Infrastructure.Mapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMemoryCache();

builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IAuthorizationService, AuthorizationService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IBlacklistService, BlacklistService>();
builder.Services.AddScoped<ICacheService, CacheService>();
builder.Services.AddScoped<ILoggingService, LoggingService>();
builder.Services.AddScoped<IRequestLoggingService, RequestLoggingService>();
builder.Services.AddScoped<IStatisticsService, StatisticsService>();
builder.Services.AddScoped<IStockService, StockService>();
builder.Services.AddScoped<ITradeService, TradeService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IWalletCustomService, WalletCustomService>();

builder.Services.AddScoped<IAccountClient, AccountClient>();
builder.Services.AddScoped<IUserAccountClient, UserAccountClient>();
builder.Services.AddScoped<IWalletAccountClient, WalletAccountClient>();
builder.Services.AddScoped<IAuthenticateAccountClient, AuthenticateAccountClient>();
builder.Services.AddScoped<IStockAccountClient, StockAccountClient>();
builder.Services.AddScoped<ISettlementsClient, SettlementsClient>();
builder.Services.AddScoped<IAnalyzerClient, AnalyzerClient>();
builder.Services.AddScoped<IStockClient, StockClient>();

builder.Services.AddScoped<IHttpClientFactoryCustom, HttpClientFactoryCustom>();

builder.Services.AddHttpContextAccessor()
                .Configure<HostSettings>(builder.Configuration.GetSection("HostSettings"))
                .Configure<UserSettings>(builder.Configuration.GetSection("UserSettings"))
                .Configure<AccountSettings>(builder.Configuration.GetSection("AccountSettings"))
                .Configure<StockApiSettings>(builder.Configuration.GetSection("StockApiSettings"));

builder.Services.AddAutoMapper(mc =>
{
    mc.AddProfile(new UserProfile());
});

builder.Services.AddControllers();

var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JWTSettings>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidIssuer = jwtSettings.Issuer,
            ValidAudience = jwtSettings.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key)),
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true
        };
    });


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "StockManagement Gateway", Version = "v1" });
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

var app = builder.Build();

app.UseMiddleware<ErrorHandlingMiddleware>();

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

    var loggerFactory = LoggerFactory.Create(builder =>
    {
        builder
                .AddConsole();
    });

var logger = loggerFactory.CreateLogger<Program>();

    logger.LogInformation("...");
    logger.LogWarning("Warning");
    logger.LogError("Error");

    
    var user = "Domain";
    logger.LogInformation("Consumer {User} logged in the system.", user);
