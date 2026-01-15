using smarthome_core;
using TelemetryService.Models;

namespace TelemetryService;

using MassTransit;

public class TelemetryEventConsumer(ITelemetryRepository repository, ILogger<TelemetryEventConsumer> logger) : IConsumer<DeviceEvent>
{
    public async Task Consume(ConsumeContext<DeviceEvent> context)
    {
        var data = context.Message;

        logger.LogInformation($"Telemetry event from '{data.DeviceId}'. Data: {data.Data} ");
        
        await repository.SaveAsync(new DeviceTelemetry
        {
            DeviceId = data.DeviceId,
            Data = data.Data,
            ReceivedAt = data.ReceivedAt,
        });
    }
}