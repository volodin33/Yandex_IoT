using System.Reflection;
using device_registry;
using device_registry.DB;
using device_registry.Models.Db;
using Microsoft.EntityFrameworkCore;
using smarthome_core;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<AuthorizeFilter>();
builder.Services.AddScoped<CurrentUser>();

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddMediatR(cfg => { cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()); });
builder.Services.AddAutoMapper(cfg =>
{
    cfg.CreateMap<DeviceInfo, DeviceEntity>().ReverseMap();
});
builder.Services.AddPostrgresDb<DeviceRegistryDbContext>(builder.Configuration);
builder.Services.AddRabbitMq(builder.Configuration).Complete();
builder.Services.AddHealthChecks();

var app = builder.Build();

if (app.Configuration.GetValue<bool>("IS_DEMO_MODE"))
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<DeviceRegistryDbContext>();
    db.Database.Migrate();
    
    if (!db.Devices.Any())
    {
        await DemoInitializer.Initialize(db);
    }
}

app.MapOpenApi();
app.MapHealthChecks("/health");

app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/openapi/v1.json", "My API v1");
    options.RoutePrefix = "swagger";
});

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();