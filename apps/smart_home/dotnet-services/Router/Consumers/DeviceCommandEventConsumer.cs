using MassTransit;
using MediatR;
using Router.Models.Command;
using smarthome_core;

namespace Router;

public class DeviceCommandEventConsumer(IMediator mediator): IConsumer<DeviceCommandEvent>
{
    public async Task Consume(ConsumeContext<DeviceCommandEvent> context)
    {
        var message = context.Message;
        await mediator.Send(new DeviceCommandRequest(message.DeviceId, message.Command));
    }
}