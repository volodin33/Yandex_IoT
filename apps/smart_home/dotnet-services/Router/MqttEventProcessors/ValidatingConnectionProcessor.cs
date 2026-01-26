using MQTTnet.Protocol;
using MQTTnet.Server;

namespace Router;

public class ValidatingConnectionProcessor(AuthorizedDevices authDevList)
{
    public Task Processing(ValidatingConnectionEventArgs e)
    {
        if (!authDevList.IsAuthorized(e.ClientId))
        {
            e.ReasonCode = MqttConnectReasonCode.ClientIdentifierNotValid;
        }
        return Task.CompletedTask;
    }
}