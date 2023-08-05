using Application.Contracts;
using Application.Contracts;
using Application.Services;
using Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Application
{
    public static class ServiceCollectionExtension
    {
        public static void AddApplicationServices(this IServiceCollection services)
        {

            // services.AddScoped<IFeatureRepository, FeatureRepository>();
            // services.AddScoped<IFeatureService, FeatureService>();
            // services.AddScoped<IAuthService, AuthService>();
        }
    }
}
