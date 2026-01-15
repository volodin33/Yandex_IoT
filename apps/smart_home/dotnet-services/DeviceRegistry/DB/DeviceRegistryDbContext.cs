using device_registry.Models.Db;
using Microsoft.EntityFrameworkCore;

namespace device_registry.DB;

public class DeviceRegistryDbContext(DbContextOptions<DeviceRegistryDbContext> options) : DbContext(options)
{
    public DbSet<DeviceEntity> Devices => Set<DeviceEntity>();
}