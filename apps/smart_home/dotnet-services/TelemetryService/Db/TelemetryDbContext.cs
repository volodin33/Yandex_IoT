using Microsoft.EntityFrameworkCore;
using TelemetryService.Models.DB;

namespace TelemetryService.Db;

public class TelemetryDbContext(DbContextOptions<TelemetryDbContext> options) : DbContext(options)
{
    public DbSet<DeviceTelemetryEntity> DeviceTelemetry => Set<DeviceTelemetryEntity>();
}