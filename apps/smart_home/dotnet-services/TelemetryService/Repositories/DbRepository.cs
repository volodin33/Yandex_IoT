using TelemetryService.Db;
using TelemetryService.Models;
using TelemetryService.Models.DB;

namespace TelemetryService.Repositories;

public class DbRepository(TelemetryDbContext db) : ITelemetryRepository
{
    public async Task SaveAsync(DeviceTelemetry data)
    {
        db.DeviceTelemetry.Add(new DeviceTelemetryEntity
        {
            DeviceId = data.DeviceId,
            Data = data.Data,
            ReceivedAt = data.ReceivedAt,
        });
        
        await db.SaveChangesAsync();
    }
}