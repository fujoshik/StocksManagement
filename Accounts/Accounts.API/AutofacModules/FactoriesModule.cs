using Accounts.Domain.DTOs;
using Autofac;

namespace Accounts.API.AutofacModules
{
    public class FactoriesModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var assembly = typeof(BaseResponseDto).Assembly;

            builder.RegisterAssemblyTypes(assembly)
                    .Where(t => t.Name.EndsWith("Factory"))
                    .AsImplementedInterfaces()
                    .InstancePerLifetimeScope();
        }
    }
}
