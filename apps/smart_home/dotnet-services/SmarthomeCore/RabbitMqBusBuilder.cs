using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace smarthome_core;

public class RabbitMqBusBuilder(IServiceCollection services, IConfiguration configure)
{
    private readonly List<Type> _consumerTypes = [];
    private readonly List<Action<IRegistrationContext, IRabbitMqBusFactoryConfigurator>> _registrations = [];

    public RabbitMqBusBuilder WithConsumer<T>(string queueName) where T : class, IConsumer
    {
        _consumerTypes.Add(typeof(T));
        
        _registrations.Add((ctx, rabbitCfg) =>
        {
            rabbitCfg.ReceiveEndpoint(queueName, e =>
            {
                e.ConfigureConsumer<T>(ctx); 
            });
        });
        return this;
    }

    public void Complete()
    {
        var conStr = configure.GetConnectionString("RabbitMq");
        
        services.AddMassTransit(x =>
        {
            foreach (var type in _consumerTypes)
            {
                x.AddConsumer(type);
            }
            
            x.UsingRabbitMq((ctx, cfg) =>
            {
                cfg.Host(new Uri(conStr));

                foreach (var reg in _registrations)
                {
                    reg(ctx, cfg);
                }
            });
        });
    }
}