using FlowService.Models.Commands;
using FlowService.Models.Db;
using MediatR;
using Newtonsoft.Json;

namespace FlowService.Handlers;

public class CreateFlowReuestHandler(FlowDbContext db) : IRequestHandler<CreateFlowReuest>
{
    public async Task Handle(CreateFlowReuest request, CancellationToken cancellationToken)
    {
        await db.Flows.AddAsync(new FlowEntity
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            TriggerDeviceId = request.TriggerDeviceId,
            FlowJson = JsonConvert.SerializeObject(request.FlowData)
        }, 
            cancellationToken);
        
        await db.SaveChangesAsync(cancellationToken);
    }
}