using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using MQTTnet;

await Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
{
    var configuration = hostContext.Configuration;
    services.AddOptions<MqttOptions>()
        .Bind(configuration.GetSection("Mqtt"));

    services.AddSingleton<MqttClientFactory>();
    services.AddSingleton<IMqttClient>(sp =>
        sp.GetRequiredService<MqttClientFactory>().CreateMqttClient());
    services.AddHostedService<DeviceEmulator>();
})
.Build().RunAsync();