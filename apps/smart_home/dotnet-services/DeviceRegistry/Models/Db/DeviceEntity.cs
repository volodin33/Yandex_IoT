using smarthome_core;

namespace device_registry.Models.Db;

public class DeviceEntity
{
    public Guid Id { get; set; }
    public string DeviceId { get; set; }
    public string UserId { get; set; }
    public string Type { get; set; }
    public DeviceState State { get; set; }
    public string Description { get; set; }
}