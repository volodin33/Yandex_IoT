using MassTransit;
using smarthome_core;

namespace Router;

public class DeviceInfoEventConsumer(AuthorizedDevices authorizedDevices): IConsumer<DeviceInfoEvent>
{
    public Task Consume(ConsumeContext<DeviceInfoEvent> context)
    {
        authorizedDevices.UpdateDevice( context.Message.Info);
        return Task.CompletedTask;
    }
}