using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Core;

public static class Config
{
    public static IServiceCollection AddCoreService(this IServiceCollection services)
    {
        return services;
    }

    private static IServiceCollection AddMediatR(this IServiceCollection services)
    {
        return services.AddScoped<IMediator, Mediator>()
            .AddTransient<ServiceFactory>(sp => sp.GetRequiredService!);
    }
}