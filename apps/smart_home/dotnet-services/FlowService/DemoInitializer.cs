using FlowService.Models;
using FlowService.Models.Db;
using Newtonsoft.Json;
using smarthome_core;

namespace FlowService;

public static class DemoInitializer
{
    public static async Task Initialize(FlowDbContext db)
    {
        db.Flows.AddRange(new FlowEntity
        {
            Id = Guid.NewGuid(),
            TriggerDeviceId = DemoData.TemperatureSensorId,
            Name = "Kitchen_TemperatureControl",
            IsEnabled = true,
            FlowJson = JsonConvert.SerializeObject(new Flow
            {
                Condition = new()
                {
                    Operator = "<",
                    Value = 23,
                },
                Action = new ()
                {
                    TargetDeviceId = DemoData.HeaterId,
                    Command = "ON",
                }
            })
        },
        new FlowEntity{
            Id = Guid.NewGuid(),
            TriggerDeviceId = DemoData.TemperatureSensorId,
            Name = "Kitchen_TemperatureControl",
            IsEnabled = true,
            FlowJson = JsonConvert.SerializeObject(new Flow
            {
                Condition = new()
                {
                    Operator = ">",
                    Value = 26,
                },
                Action = new ()
                {
                    TargetDeviceId = DemoData.HeaterId,
                    Command = "OFF",
                }
            })
        });
        
        await db.SaveChangesAsync();
    }
}