using Infrastructure.Authorization;
using Infrastructure.Contracts;
using Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class ServiceCollectionExtension
{
    public static void AddInfrastructureServices(this IServiceCollection services)
    {

        services.AddScoped<ISessionRepository, SessionRepository>();
        services.AddScoped<IPermissionService, PermissionService>();
    }
}