using System.ComponentModel.DataAnnotations.Schema;

namespace FlowService.Models.Db;

public class FlowEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string TriggerDeviceId { get; set; } = string.Empty;

    [Column(TypeName = "jsonb")] 
    public string FlowJson { get; set; } = "{}";
    
    public bool IsEnabled { get; set; } = true;
}