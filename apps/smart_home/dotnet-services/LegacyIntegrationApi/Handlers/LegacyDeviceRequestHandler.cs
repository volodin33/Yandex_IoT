using LegacyIntegrationApi.Models.Commands;
using MassTransit;
using MediatR;
using smarthome_core;

namespace LegacyIntegrationApi.Handlers;

public class LegacyDeviceRequestHandler(IPublishEndpoint publishEndpoint) : IRequestHandler<LegacyDeviceRequest>
{
    public async Task Handle(LegacyDeviceRequest request, CancellationToken cancellationToken)
    {
        await publishEndpoint.Publish(new LegacyDeviceEvent
        {
            DeviceName = request.DeviceName,
            Data = request.Data,
        }, cancellationToken);
    }
}