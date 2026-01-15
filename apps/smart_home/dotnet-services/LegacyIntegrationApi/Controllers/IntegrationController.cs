using LegacyIntegrationApi.Models.API;
using LegacyIntegrationApi.Models.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LegacyIntegrationApi.Controllers;

[ApiController]
[Route("[controller]")]
public class IntegrationController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Send([FromBody]LegacyDeviceApiRequest request)
    {
        await mediator.Send(new LegacyDeviceRequest
            {
                DeviceName = request.DeviceName,
                Data = request.Data,
            }
        );
        
        return NoContent();
    }
}