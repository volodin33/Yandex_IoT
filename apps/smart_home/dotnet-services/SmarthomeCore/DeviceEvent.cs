namespace smarthome_core;

public class DeviceEvent
{
    public string DeviceId { get; set; }
    public string Data { get; set; }
    public DateTimeOffset? ReceivedAt { get; set; }
}

public class DeviceCommandEvent
{
    public string DeviceId { get; set; }
    public string Command { get; set; }
}