using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace GameAPI.Messaging;

public class MessageService
{
    private readonly IModel channel;
    private readonly IConfiguration configuration;

    public MessageService(IConfiguration configuration)
    {
        this.configuration = configuration;
        IConnection connection = Connect();
        channel = connection.CreateModel();
        Console.WriteLine("Messaging Online!");
    }

    public void Publish(MessageData data)
    {
        channel.ExchangeDeclare(data.ExchangeName, ExchangeType.Direct);
        var jsonMessage = JsonSerializer.Serialize(data);
        var body = Encoding.UTF8.GetBytes(jsonMessage);
        channel.BasicPublish(data.ExchangeName, data.RoutingKey, null, body);
    }

    public string Subscribe(MessageData message, Action<MessageData> handler)
    {
        channel.ExchangeDeclare(message.ExchangeName, ExchangeType.Direct);
        channel.QueueDeclare(message.QueueName, true, false, false, null);
        channel.QueueBind(message.QueueName, message.ExchangeName, message.RoutingKey);

        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var receivedMessage = Encoding.UTF8.GetString(body);
            Console.WriteLine(receivedMessage);

            MessageData? response = JsonSerializer.Deserialize<MessageData>(receivedMessage);
            handler(response ?? new() { Data = "Err" });
        };

        return channel.BasicConsume(message.QueueName, true, consumer);
    }

    public void UnSubscribe(string tag)
    {
        channel.BasicCancel(tag);
    }

    private IConnection Connect()
    {
        var factory = new ConnectionFactory
        {
            Uri = new(configuration.GetConnectionString("RabbitMQContext") ?? string.Empty)
        };

        return factory.CreateConnection();
    }
}
