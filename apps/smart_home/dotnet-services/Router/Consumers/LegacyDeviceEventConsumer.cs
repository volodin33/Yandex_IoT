using MassTransit;
using smarthome_core;

namespace Router;

public class LegacyDeviceEventConsumer(AuthorizedDevices authorizedDevices, IPublishEndpoint rabbitEndPoint): IConsumer<LegacyDeviceEvent>
{
    public async Task Consume(ConsumeContext<LegacyDeviceEvent> context)
    {
        var message = context.Message;
        if (!authorizedDevices.IsAuthorized(message.DeviceName))
        {
            throw new Exception($"No authorized device: {message.DeviceName}");
        }
        
        await rabbitEndPoint.Publish(new DeviceEvent
        {
            DeviceId = message.DeviceName,
            Data = message.Data,
            ReceivedAt = DateTimeOffset.UtcNow,
        });
    }
}