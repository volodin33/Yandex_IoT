using MediatR;
using smarthome_core;

namespace device_registry.Models.Comands;

public record GetDeviceRequest(List<string>? DeviceIds = null) : IRequest<List<DeviceInfo>>;