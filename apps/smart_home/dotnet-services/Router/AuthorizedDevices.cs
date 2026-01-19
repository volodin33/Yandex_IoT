using System.Collections.Concurrent;
using MassTransit;
using smarthome_core;

namespace Router;

public class AuthorizedDevices
{
    private readonly ConcurrentDictionary<string, DeviceInfo?> _deviceCache = new();

    public void Init(IEnumerable<DeviceInfo> devices)
    {
        foreach (var device in devices)
            _deviceCache.TryAdd(device.DeviceId, device);
    }

    public bool IsAuthorized(string deviceId)
    {
        _deviceCache.TryGetValue(deviceId, out var device);
        return device is {State: DeviceState.Enabled} or {State: DeviceState.Added};
    }

    public DeviceInfo? GetDevice(string deviceId)
    {
        _deviceCache.TryGetValue(deviceId, out var device);
        return device;
    }

    public void UpdateDevice(DeviceInfo info)
    {
        if (info.State == DeviceState.Removed)
        {
            _deviceCache.TryRemove(info.DeviceId, out _);
        }
        else
        {
            _deviceCache.AddOrUpdate(info.DeviceId, info, (_, _) => info);
        }
    }
}