namespace FlowService.Models.API;

public class CreateFlowApiRequest
{
    public string Name { get; set; } = string.Empty;
    public string TriggerDeviceId { get; set; } = string.Empty;
    public Flow FlowData { get; set; } = new(); 
}