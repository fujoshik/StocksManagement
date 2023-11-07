using Accounts.Infrastructure.Mapper;

namespace Accounts.API.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddStocksAutomapper(this IServiceCollection services)
            => services.AddAutoMapper(mc =>
            {
                mc.AddProfile(new AccountProfile());
                mc.AddProfile(new UserProfile());
                mc.AddProfile(new WalletProfile());
            });
    }
}
