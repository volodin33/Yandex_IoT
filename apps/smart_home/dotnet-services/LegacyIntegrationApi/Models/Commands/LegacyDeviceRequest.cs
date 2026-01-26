using MediatR;

namespace LegacyIntegrationApi.Models.Commands;

public class LegacyDeviceRequest : IRequest
{
    public string DeviceName { get; set; }
    public string Data { get; set; }
}