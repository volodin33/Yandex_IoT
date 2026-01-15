namespace FlowService.Models;

public class Flow
{
    public FlowCondition Condition { get; set; } = new();
    public FlowAction Action { get; set; } = new();
}

public class FlowCondition
{
    public string Property { get; set; } = string.Empty;
    public string Operator { get; set; } = string.Empty;
    public double Value { get; set; }
}

public class FlowAction
{
    public string TargetDeviceId { get; set; } = string.Empty;
    public string Command { get; set; } = string.Empty;
}