using Microsoft.Extensions.Diagnostics.HealthChecks;
using temperature_api;

var builder = WebApplication.CreateBuilder(args);

builder.Services    
    .AddControllers()
    .AddJsonOptions(o => o.JsonSerializerOptions.WriteIndented = true);

builder.Services
    .AddOptions<TemperatureSettings>()
    .Bind(builder.Configuration.GetSection("Temperature"));

builder.Services
    .AddHealthChecks()
    .AddCheck("self", () => HealthCheckResult.Healthy());

var app = builder.Build();

// Configure the HTTP request pipeline.
/*
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
*/
app.UseAuthorization();
app.MapControllers();
app.MapHealthChecks("/health");

app.Run();