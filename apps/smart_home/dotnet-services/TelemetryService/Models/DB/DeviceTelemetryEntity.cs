namespace TelemetryService.Models.DB;

public class DeviceTelemetryEntity
{
    public long Id { get; set; }
    public string DeviceId { get; set; }
    public string Data { get; set; }
    public DateTimeOffset? ReceivedAt { get; set; }
}