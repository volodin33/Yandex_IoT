namespace smarthome_core;

public class DeviceInfo
{
    public string DeviceId { get; set; }
    public string UserId { get; set; }
    public string Type { get; set; }
    public DeviceState State { get; set; }
    public string Description { get; set; }
}

public class DeviceInfoEvent
{
    public Guid? Id { get; set; }
    public DeviceInfo Info { get; set; }
}

public enum DeviceState
{
    Added,
    Enabled,
    Disabled,
    Removed,
}

public record GetDeviceRequest(List<string>? DeviceIds = null);