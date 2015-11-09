using Microsoft.AspNet.Identity;
using Microsoft.Framework.DependencyInjection;
using Microsoft.Framework.DependencyInjection.Extensions;
using TokenAuthExampleWebApplication.Authentication;

namespace TokenAuthExampleWebApplication.Extensions
{
    public static class ConfigureServicesExtensions
    {
        public static void AddCustomIdentity<TUser>(this IServiceCollection services) where TUser : BasicUser
        {
            services.TryAddScoped<IPasswordHasher<TUser>, PasswordHasher<TUser>>();
            services.TryAddScoped<IUserManager<TUser>, CustomUserManager<TUser>>();
            services.TryAddScoped<ISignInManager<TUser>, CustomSignInManager<TUser>>();
        }
    }
}
