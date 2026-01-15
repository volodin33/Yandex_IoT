using FlowService.Models;
using MassTransit;
using Newtonsoft.Json;
using smarthome_core;

namespace FlowService;

public class FlowEngine(FlowDbContext db, IPublishEndpoint publishEndpoint)
{
    public async Task HandleEventAsync(string deviceId, string value)
    {
        var flows = db.Flows
            .Where(f => f.TriggerDeviceId == deviceId && f.IsEnabled)
            .ToList();

        foreach (var flowEntity in flows)
        {
            var data = JsonConvert.DeserializeObject<Flow>(flowEntity.FlowJson);
            if (data != null && EvaluateCondition(data.Condition, value))
            {
                await publishEndpoint.Publish(new DeviceCommandEvent
                {
                    DeviceId = data.Action.TargetDeviceId,
                    Command = data.Action.Command,
                });
            }
        }
    }

    private bool EvaluateCondition(FlowCondition condition, string value)
    {
        if (double.TryParse(value.Trim(), out var digitValue))
        {
            return condition.Operator switch
            {
                ">" => digitValue > condition.Value,
                "<" => digitValue < condition.Value,
                "==" => Math.Abs(digitValue - condition.Value) < 0.01,
            };
        }
        return false;
    }
}