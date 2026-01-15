using MediatR;
using smarthome_core;

namespace device_registry.Models.Comands;

public record ManageDeviceRequest(DeviceInfo DeviceInfo) : IRequest;
