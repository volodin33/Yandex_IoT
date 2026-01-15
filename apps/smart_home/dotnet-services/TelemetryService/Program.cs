using System.Reflection;
using Microsoft.EntityFrameworkCore;
using TelemetryService;
using TelemetryService.Db;
using TelemetryService.Repositories;
using smarthome_core;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<ITelemetryRepository, DbRepository>();
builder.Services.AddPostrgresDb<TelemetryDbContext>(builder.Configuration);

builder.Services.AddLogging();
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddMediatR(cfg => { cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()); });
builder.Services.AddRabbitMq(builder.Configuration)
    .WithConsumer<TelemetryEventConsumer>("telemetry-service-queue")
    .Complete();

var app = builder.Build();

if (app.Configuration.GetValue<bool>("IS_DEMO_MODE"))
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<TelemetryDbContext>();
    await db.Database.MigrateAsync();
}

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();