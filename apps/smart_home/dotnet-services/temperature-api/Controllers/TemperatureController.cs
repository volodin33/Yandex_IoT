using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace temperature_api.Controllers;

[ApiController]
[Route("temperature")]
public class TemperatureController(IOptions<TemperatureSettings> options) : ControllerBase
{
    private readonly double _temperatureMin = options.Value.Min;
    private readonly double _temperatureMax = options.Value.Max;

    [HttpGet]
    public ActionResult<TemperatureResponse> Get([FromQuery] string? location)
    {
        if (string.IsNullOrWhiteSpace(location))
        {
            return BadRequest("Query parameter 'location' is required.");
        }

        return Ok(new TemperatureResponse
        {
            Location =  location,
            Value =  Random.Shared.NextDouble() * (_temperatureMax - _temperatureMin) + _temperatureMin,
            Timestamp = DateTime.UtcNow,
        });
    }
}