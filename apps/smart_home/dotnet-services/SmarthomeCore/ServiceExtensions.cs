using System.Reflection;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace smarthome_core;

public static class ServiceExtensions
{
   public static RabbitMqBusBuilder AddRabbitMq(this IServiceCollection services, IConfiguration configure)
        => new (services, configure);

    public static IServiceCollection AddPostrgresDb<T>(this IServiceCollection services, IConfiguration configure, string? connectionStringName = null)
        where T : DbContext
    {
        var conStr = configure.GetConnectionString(connectionStringName ?? "Db");
        return services.AddDbContext<T>(options => options.UseNpgsql(conStr));
    }
}