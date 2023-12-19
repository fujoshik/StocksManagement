using Settlement.Domain;
using Settlement.Domain.Abstraction.Repository;
using Settlement.Domain.Abstraction.Routes;
using Settlement.Domain.Abstraction.Services;
using Settlement.Domain.Dependencies;
using Settlement.Domain.Services;
using Settlement.Infrastructure.Repository;

namespace Settlement.API.Extensions
{
    public static class ServiceConfigurationExstensions
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<ISettlementService, SettlementService>();
            services.AddScoped<IHttpClientService, ConnectionService>();
            services.AddScoped<IWalletRoutes, WalletRoutes>();
            services.AddScoped<IStockRoutes, StockRoutes>();
            services.AddScoped<ISettlementRepository, SettlementRepository>();
            services.AddScoped<IWalletCacheService, WalletCacheService>();
            services.AddScoped<ISettlementDependencies, SettlementDependencies>();
            services.AddScoped<IConnectionDependencies, ConnectionDependencies>();
        }
    }
}
