using Core.Commands;
using Microsoft.Extensions.DependencyInjection;
using Tickets.Maintainance.Commands;

namespace Tickets.Maintainance;

internal static class Config
{
    internal static void AddMaintainance(this IServiceCollection services)
    {
        AddCommandHandlers(services);
    }

    private static void AddCommandHandlers(IServiceCollection services)
    {
        services.AddCommandHandler<RebuildProjection, MaintenanceCommandHandler>();
    }
}