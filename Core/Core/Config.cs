using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Core;

public static class Config
{
    public static IServiceCollection AddCoreService(this IServiceCollection services)
    {
        services.AddMediatR()
        //    .AddScoped<ICommandBus, CommandBus>()
        //    .AddScoped<IQueryBus, QueryBus>()
        ;

        //services.TryAddScoped<IEventBus, EventBus>();
        //services.TryAddScoped<IExternalEventProducer, NulloExternalEventProducer>();
        //services.TryAddScoped<IExternalCommandBus, ExternalCommandBus>();

        //services.TryAddScoped<IIdGenerator, NulloIdGenerator>();

        return services;
    }

    private static IServiceCollection AddMediatR(this IServiceCollection services)
    {
        return services.AddScoped<IMediator, Mediator>()
            .AddTransient<ServiceFactory>(sp => sp.GetRequiredService!);
    }
}