using System.Reflection;
using FlowService;
using Microsoft.EntityFrameworkCore;
using smarthome_core;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddMediatR(cfg => { cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()); });
builder.Services.AddRabbitMq(builder.Configuration)
    .WithConsumer<DeviceEventConsumer>("flow-service-device-event-queue")
    .Complete();

builder.Services.AddPostrgresDb<FlowDbContext>(builder.Configuration);

builder.Services.AddScoped<FlowEngine>();

var app = builder.Build();

if (app.Configuration.GetValue<bool>("IS_DEMO_MODE"))
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<FlowDbContext>();
    db.Database.Migrate();
    
    if (!db.Flows.Any())
    {
        await DemoInitializer.Initialize(db);
    }
}

app.MapOpenApi();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();