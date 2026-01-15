using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using MQTTnet;

public class DeviceEmulator(IMqttClient client, IOptions<MqttOptions> options) : BackgroundService
{
    private readonly MqttOptions _options = options.Value;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Console.WriteLine("Запуск эмулятора");
        
        var server = _options.Server;
        var topic = _options.Topic;
        var interval = _options.Interval;
        
        var clientOptions = new MqttClientOptionsBuilder()
            .WithTcpServer(server)
            .WithClientId(_options.ClientId)
            .Build();

        var result = await client.ConnectAsync(clientOptions, stoppingToken);
        if (result.ResultCode != MqttClientConnectResultCode.Success)
        {
            Console.WriteLine($"Ошибка подключения: {result.ReasonString}");
            return;
        }

        var timer = new PeriodicTimer(TimeSpan.FromSeconds(interval));
        var rnd = new Random();

        try
        {
            while (await timer.WaitForNextTickAsync(stoppingToken))
            {
                var temperature = 20 + rnd.NextDouble() * 5;
                var payload = $"{temperature:F1}";

                var message = new MqttApplicationMessageBuilder()
                    .WithTopic(topic)
                    .WithPayload(payload)
                    .Build();

                await client.PublishAsync(message, stoppingToken);
                Console.WriteLine($"Значение датчика: {payload}");
            }
        }
        catch (OperationCanceledException)
        {
            Console.WriteLine("Остановка эмулятора");
        }
        finally
        {
            if (client.IsConnected)
                await client.DisconnectAsync();
        }
    }
}