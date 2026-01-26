namespace TelemetryService.Models;

public class DeviceTelemetry
{
    public string DeviceId { get; set; }
    public string Data { get; set; }
    public DateTimeOffset? ReceivedAt { get; set; }
}