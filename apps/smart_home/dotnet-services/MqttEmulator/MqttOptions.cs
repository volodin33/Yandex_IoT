public class MqttOptions
{
    public string Server { get; set; }
    public string Topic { get; set; }
    
    public string ClientId { get; set; }
    public int Interval { get; set; }
}