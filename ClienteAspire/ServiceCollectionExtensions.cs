using FBS_ComponentesDinamicos.Sevices;

namespace ClienteAspire
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection DependencyInjectionConfiguration(this IServiceCollection services)
        {
            services.AddScoped<ILocalStorageService, LocalStorageService>();
            return services;
        }
    }
}
