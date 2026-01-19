using FlowService.Models.Db;
using Microsoft.EntityFrameworkCore;

namespace FlowService;

public class FlowDbContext(DbContextOptions<FlowDbContext> options) : DbContext(options)
{
    public DbSet<FlowEntity> Flows => Set<FlowEntity>();
}