using Refit;
using smarthome_core;

namespace Router;

public interface IDeviceRegistryApi
{
    [Headers("user-id: router-service")]
    [Post("/DeviceRegistry/query")]
    Task<IEnumerable<DeviceInfo>> GetDevices(GetDeviceRequest request);
}