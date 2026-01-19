using smarthome_core;

namespace device_registry.Models.API;

public record CreateDeviceApiRequest(
    string DeviceId, 
    string Description, 
    string Type
);