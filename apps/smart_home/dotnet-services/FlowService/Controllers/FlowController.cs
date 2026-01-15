using FlowService.Models.API;
using FlowService.Models.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FlowService.Controllers;

[ApiController]
[Route("[controller]")]
public class FlowController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateFlowApiRequest apiRequest)
    {
        await mediator.Send(new CreateFlowReuest(
             apiRequest.Name, 
             apiRequest.TriggerDeviceId, 
             apiRequest.FlowData
        ));
        
        return Ok();
    }
}