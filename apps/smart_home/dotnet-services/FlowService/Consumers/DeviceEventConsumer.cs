using smarthome_core;
using MassTransit;

namespace FlowService;

public class DeviceEventConsumer(FlowEngine engine) : IConsumer<DeviceEvent>
{
    public async Task Consume(ConsumeContext<DeviceEvent> context)
    {
        var data = context.Message;
        await engine.HandleEventAsync(data.DeviceId, data.Data);
    }
}