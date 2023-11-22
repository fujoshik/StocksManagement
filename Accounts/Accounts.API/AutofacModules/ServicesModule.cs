using Accounts.Domain.Abstraction.Services;
using Accounts.Domain.Services;
using Autofac;

namespace Accounts.API.AutofacModules
{
    public class ServicesModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var assembly = typeof(AuthenticationService).Assembly;

            builder.RegisterAssemblyTypes(assembly)
                    .Where(t => t.Name.EndsWith("Service"))
                    .AsImplementedInterfaces()
                    .InstancePerLifetimeScope();

            builder.RegisterType<TokenService>()
                   .As<ITokenService>()
                   .WithParameter("JwtSettings", "");
        }
    }
}
