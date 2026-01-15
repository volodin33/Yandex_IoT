using device_registry.DB;
using device_registry.Models.Db;
using smarthome_core;

namespace device_registry;

public static class DemoInitializer
{
    public static async Task Initialize(DeviceRegistryDbContext db)
    {
        db.Devices.AddRange(
            new DeviceEntity
            {
                Id = Guid.NewGuid(),
                DeviceId = DemoData.TemperatureSensorId,
                State = DeviceState.Added,
                Type = "TemperatureSensor",
                Description = "Температура на кухне",
                UserId = DemoData.UserId,
            },
            new DeviceEntity
            {
                Id = Guid.NewGuid(),
                DeviceId = DemoData.HeaterId,
                State = DeviceState.Added,
                Type = "Heater",
                Description = "Обогреватель на кухне",
                UserId = DemoData.UserId,
            });
        
        await db.SaveChangesAsync();
    }
}