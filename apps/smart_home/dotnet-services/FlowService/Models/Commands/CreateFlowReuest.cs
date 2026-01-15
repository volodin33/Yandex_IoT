using MediatR;

namespace FlowService.Models.Commands;

public record CreateFlowReuest(string Name, string TriggerDeviceId, Flow FlowData) : IRequest;