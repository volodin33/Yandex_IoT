using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using MQTTnet.Server;
using smarthome_core;

namespace Router;

public class StartupManager(
    MqttServer server, 
    IDeviceRegistryApi deviceApi, 
    AuthorizedDevices cache) : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var devices = await deviceApi.GetDevices(new GetDeviceRequest());
        cache.Init(devices);
        await server.StartAsync();
    }

    public async Task StopAsync(CancellationToken cancellationToken) => await server.StopAsync();
}