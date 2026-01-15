using System;
using System.Text.Json.Serialization;

namespace temperature_api;

public sealed class TemperatureResponse
{
    [JsonPropertyName("value")]
    public double Value { get; init; }

    [JsonPropertyName("unit")]
    public string Unit { get; init; }

    [JsonPropertyName("timestamp")]
    public DateTime Timestamp { get; init; } 

    [JsonPropertyName("location")]
    public string Location { get; init; } 

    [JsonPropertyName("status")]
    public string Status { get; init; }

    [JsonPropertyName("sensor_id")]
    public string SensorID { get; init; }

    [JsonPropertyName("sensor_type")]
    public string SensorType { get; init; }

    [JsonPropertyName("description")]
    public string Description { get; init; }
}