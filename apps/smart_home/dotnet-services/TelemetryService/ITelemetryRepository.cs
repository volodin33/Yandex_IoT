using smarthome_core;
using TelemetryService.Models;

namespace TelemetryService;

public interface ITelemetryRepository
{
    Task SaveAsync(DeviceTelemetry data);
}