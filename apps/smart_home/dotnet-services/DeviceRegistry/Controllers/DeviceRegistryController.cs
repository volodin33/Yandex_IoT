using device_registry.Models.API;
using device_registry.Models.Comands;
using device_registry.Models.Db;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using smarthome_core;
using GetDeviceRequest = device_registry.Models.Comands.GetDeviceRequest;

namespace device_registry.Controllers;

[ApiController]
[Route("[controller]")]
[ServiceFilter(typeof(AuthorizeFilter))]
public class DeviceRegistryController(CurrentUser currentUser, IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Добавляет новое устройство.
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create(CreateDeviceApiRequest deviceApiRequest)
    {
        await mediator.Send(new ManageDeviceRequest
        (
            new DeviceInfo
            {
                DeviceId = deviceApiRequest.DeviceId,
                State = DeviceState.Added,
                Description = deviceApiRequest.Description,
                Type = deviceApiRequest.Type,
                UserId = currentUser.Id
            }
        ));
        
        return Created();
    }

    /// <summary>
    /// Удаляет устройство.
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Remove(string id)
    {
        await mediator.Send(new ManageDeviceRequest
        (
            new DeviceInfo
            {
                UserId = currentUser.Id, 
                DeviceId = id,
                State = DeviceState.Removed, 
            }
        ));
        return NoContent();
    }
    
    /// <summary>
    /// Отключает устройство.
    /// </summary>
    [HttpPost("{id}/disable")]
    public async Task<IActionResult> Disable(string id)
    {
        await mediator.Send(new ManageDeviceRequest
        (
            new DeviceInfo
            {
                UserId = currentUser.Id, 
                DeviceId = id, 
                State = DeviceState.Disabled 
            }
        ));
        return NoContent();
    }
    
    /// <summary>
    /// Включает устройство.
    /// </summary>
    [HttpPost("{id}/enable")]
    public async Task<IActionResult> Enable(string id)
    {
        await mediator.Send(new ManageDeviceRequest
        (
            new DeviceInfo
            {
                UserId = currentUser.Id,
                DeviceId = id, 
                State = DeviceState.Enabled
            }
        ));
        return NoContent();
    }
    
    /// <summary>
    /// Возращает список устройств.
    /// </summary>
    [HttpPost("query")]
    public async Task<ActionResult<IEnumerable<DeviceInfo>>> GetDevices(smarthome_core.GetDeviceRequest request)
    {
        var list = await mediator.Send(new GetDeviceRequest(request.DeviceIds));
        return Ok(list);
    }
}