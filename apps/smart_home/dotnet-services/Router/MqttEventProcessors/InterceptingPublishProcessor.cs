using System.Text;
using System.Text.RegularExpressions;
using MassTransit;
using MQTTnet.Server;
using smarthome_core;

namespace Router;

public class InterceptingPublishProcessor(AuthorizedDevices authDevList, IPublishEndpoint rabbitEndPoint)
{
    private static readonly Regex TopicRegex = new(@"^devices/(?<deviceId>[^/]+)/data", RegexOptions.Compiled | RegexOptions.IgnoreCase);

    public async Task Processing(InterceptingPublishEventArgs e)
    {
        if (e.ClientId == "server")
        {
            return;
        }

        var payload = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);
        var match = TopicRegex.Match(e.ApplicationMessage.Topic);

        if (!match.Success)
        {
            e.ProcessPublish =  false;
            return;
        }
        
        var deviceId = match.Groups["deviceId"].Value;
        var deviceInfo = authDevList.GetDevice(deviceId);

        if (deviceInfo != null)
        {
            await rabbitEndPoint.Publish(new DeviceEvent
            {
                DeviceId = deviceId,
                Data = payload,
                ReceivedAt = DateTimeOffset.UtcNow,
            });
        }
    }
}