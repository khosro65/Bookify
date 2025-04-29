using Bookify.Domain.Bookings;
using Microsoft.Extensions.DependencyInjection;

namespace Bookify.Application;

public static class DependencyInjections
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(cfg => {
            cfg.RegisterServicesFromAssembly(typeof(DependencyInjections).Assembly);
        });

        services.AddTransient<PricingService>();

        return services;
    }
}
