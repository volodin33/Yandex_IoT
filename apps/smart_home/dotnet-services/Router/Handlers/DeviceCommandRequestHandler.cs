using MediatR;
using MQTTnet;
using MQTTnet.Server;
using Router.Models.Command;

namespace Router.Handlers;

public class DeviceCommandRequestHandler(MqttServer mqttServer) : IRequestHandler<DeviceCommandRequest>
{
    public async Task Handle(DeviceCommandRequest request, CancellationToken cancellationToken)
    {
        var msg = new MqttApplicationMessageBuilder()
            .WithTopic($"devices/{request.DeviceId}/cmd")
            .WithPayload(request.Command)
            .Build();

        await mqttServer.InjectApplicationMessage(
            new InjectedMqttApplicationMessage(msg)
            {
                SenderClientId = "server"
            },
            cancellationToken);
    }
}