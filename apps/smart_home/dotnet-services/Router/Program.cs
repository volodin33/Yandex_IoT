using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using MQTTnet.Server;
using Refit;
using Router;
using smarthome_core;

await Host.CreateDefaultBuilder(args)
    .ConfigureServices((ctx, services) =>
    {
        var devRegistrUrl = ctx.Configuration.GetConnectionString("DeviceRegistry");
        
        services.AddScoped<DeviceInfoEventConsumer>();
        services.AddSingleton<AuthorizedDevices>();
        services.AddSingleton<MqttServerFactory>(); 
        services.AddSingleton<InterceptingPublishProcessor>();
        services.AddSingleton<ValidatingConnectionProcessor>();
        
        services.AddSingleton<MqttServer>(sp =>
        {
            var optionsBuilder = new MqttServerOptionsBuilder().WithDefaultEndpoint();
            var options = optionsBuilder.Build();
            
            var publishProcessor = sp.GetRequiredService<InterceptingPublishProcessor>();
            var validateConnectionProcessor = sp.GetRequiredService<ValidatingConnectionProcessor>();
            var server = sp.GetRequiredService<MqttServerFactory>().CreateMqttServer(options);

            server.ValidatingConnectionAsync += validateConnectionProcessor.Processing;
            server.InterceptingPublishAsync += publishProcessor.Processing;

            return server;
        });

        services.AddMediatR(cfg => { cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()); });
        services.AddRabbitMq(ctx.Configuration)
            .WithConsumer<DeviceInfoEventConsumer>("router-service-device-registry-event-queue")
            .WithConsumer<DeviceCommandEventConsumer>("router-service-device-command-event-queue")
            .WithConsumer<LegacyDeviceEventConsumer>("legacy-device-event-queue")
            .Complete();
       
        services.AddRefitClient<IDeviceRegistryApi>().ConfigureHttpClient((sp, client) =>
        {
            client.BaseAddress = new Uri(devRegistrUrl);
        });
        
        services.AddHostedService<StartupManager>();
    })
    .Build().RunAsync();