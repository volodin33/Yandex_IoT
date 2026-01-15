using AutoMapper;
using device_registry.DB;
using device_registry.Models.Comands;
using device_registry.Models.Db;
using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using smarthome_core;

namespace device_registry.Handlers;

public class ManageDeviceRequestHandler(DeviceRegistryDbContext db, IPublishEndpoint rabbitEndPoint, IMapper mapper) : IRequestHandler<ManageDeviceRequest>
{
    public async Task Handle(ManageDeviceRequest command, CancellationToken cancellationToken)
    {
        var id = await Save(command);
        await rabbitEndPoint.Publish(new DeviceInfoEvent
        {
            Id = id,
            Info = command.DeviceInfo,
        }, cancellationToken);
    }

    private async Task<Guid?> Save(ManageDeviceRequest command)
    {
        var deviceInfo = command.DeviceInfo;
        var device = await db.Devices
            .FirstOrDefaultAsync(d => d.DeviceId == deviceInfo.DeviceId && d.UserId == deviceInfo.UserId);
        var id = device?.Id;
        
        switch (deviceInfo.State)
        {
            case DeviceState.Added when device == null:
            {
                var entity = mapper.Map<DeviceEntity>(deviceInfo);
                entity.Id = Guid.NewGuid();
                id = entity.Id;
                await db.Devices.AddAsync(entity);
                break;
            }
            
            case DeviceState.Added:
                mapper.Map(deviceInfo, device);
                break;
            
            case DeviceState.Removed when device != null:
                db.Devices.Remove(device);
                break;
            
            case DeviceState.Removed:
                throw new KeyNotFoundException($"Устройство с ID {deviceInfo.DeviceId} не найдено.");
            
            case DeviceState.Enabled when device != null:
            case DeviceState.Disabled when device != null:
            {
                device.State = deviceInfo.State;
                break;
            }
            default:
                throw new InvalidOperationException();
        }

        await db.SaveChangesAsync();
        return id;
    }
}