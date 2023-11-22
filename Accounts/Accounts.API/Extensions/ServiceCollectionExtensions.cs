using Accounts.API.Policies.RolePolicy;
using Accounts.Domain.Constants;
using Accounts.Domain.Enums;
using Accounts.Infrastructure.Mapper;
using Microsoft.AspNetCore.Authorization;

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

        public static IServiceCollection AddPolicyBasedRoleAuthorizationServices(this IServiceCollection services)
        {
            services.AddTransient<IAuthorizationHandler, ValidRoleHandler>();
            services.AddAuthorization(authConfig =>
            {
                authConfig.AddPolicy(PolicyConstants.AllowAdminRole,
                policyBuilder => policyBuilder
                        .AddRequirements(new ValidRoleAuthorizationRequirement(Enum.GetName(typeof(Role), Role.Admin))));

                authConfig.AddPolicy(PolicyConstants.AllowAdminTrialRoles,
                policyBuilder => policyBuilder
                        .AddRequirements(new ValidRoleAuthorizationRequirement(Enum.GetName(typeof(Role), Role.Admin),
                        Enum.GetName(typeof(Role), Role.Trial))));

                authConfig.AddPolicy(PolicyConstants.AllowAdminInactiveRoles,
                policyBuilder => policyBuilder
                        .AddRequirements(new ValidRoleAuthorizationRequirement(Enum.GetName(typeof(Role), Role.Admin),
                        Enum.GetName(typeof(Role), Role.Inactive))));

                authConfig.AddPolicy(PolicyConstants.AllowAdminAndActiveRoles,
                policyBuilder => policyBuilder
                        .AddRequirements(new ValidRoleAuthorizationRequirement(Enum.GetName(typeof(Role), Role.Admin),
                        Enum.GetName(typeof(Role), Role.Regular), Enum.GetName(typeof(Role), Role.Special), 
                        Enum.GetName(typeof(Role), Role.VIP))));

                authConfig.AddPolicy(PolicyConstants.AllowAll,
                policyBuilder => policyBuilder
                        .AddRequirements(new ValidRoleAuthorizationRequirement(Enum.GetName(typeof(Role), Role.Admin),
                        Enum.GetName(typeof(Role), Role.Trial), Enum.GetName(typeof(Role), Role.Regular), 
                        Enum.GetName(typeof(Role), Role.Special), Enum.GetName(typeof(Role), Role.VIP), 
                        Enum.GetName(typeof(Role), Role.Inactive))));
            });
            return services;
        }
    }
}
