using kafka.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace kafka.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddMessageBus(this IServiceCollection services, string connection)
    {
        ArgumentNullException.ThrowIfNullOrEmpty(connection);
        services.AddSingleton<IMessageBus>(new MessageBus(connection));

        return services;
    }
}