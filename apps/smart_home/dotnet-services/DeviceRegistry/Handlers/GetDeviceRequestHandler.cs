using AutoMapper;
using device_registry.DB;
using device_registry.Models.Comands;
using device_registry.Models.Db;
using MediatR;
using Microsoft.EntityFrameworkCore;
using smarthome_core;
using GetDeviceRequest = device_registry.Models.Comands.GetDeviceRequest;

namespace device_registry.Handlers;

public class GetProductsHandler(DeviceRegistryDbContext db, IMapper mapper) : IRequestHandler<GetDeviceRequest, List<DeviceInfo>>
{
    public async Task<List<DeviceInfo>> Handle(GetDeviceRequest request, CancellationToken ct)
    {
        var query = db.Devices.AsNoTracking();
        
        if (request.DeviceIds != null && request.DeviceIds.Count != 0)
        {
            query = query.Where(p => request.DeviceIds.Contains(p.DeviceId));
        }

        return await query
            .Select(p => mapper.Map<DeviceInfo>(p))
            .ToListAsync(ct);
    }
}
