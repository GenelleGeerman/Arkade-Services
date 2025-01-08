using System.Text;
using System.Text.Json;
using BusinessLayer.Interfaces;
using BusinessLayer.Models;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;

namespace BusinessLayer.Services;

public class MessageService : IMessageService
{
    private readonly IModel channel;
    private readonly IConfiguration configuration;
    private bool isDisabled = false;

    public MessageService(IConfiguration configuration)
    {
        try
        {
            this.configuration = configuration;
            IConnection connection = Connect();
            channel = connection.CreateModel();
            Console.WriteLine("Messaging Online!");
        }
        catch (BrokerUnreachableException)
        {
            Console.WriteLine("Could not reach broker. Check if Uri is correct");
            Console.WriteLine("Disabling messaging");
            isDisabled = true;
        }
    }

    public void Publish(MessageData data)
    {
        if (isDisabled) return;
        channel.ExchangeDeclare(data.ExchangeName, ExchangeType.Direct);
        var jsonMessage = JsonSerializer.Serialize(data);
        var body = Encoding.UTF8.GetBytes(jsonMessage);
        channel.BasicPublish(data.ExchangeName, data.RoutingKey, null, body);
    }

    public string Subscribe(MessageData message, Action<MessageData> handler)
    {
        if (isDisabled)
        {
            handler(new() { Data = "No Connection" });
            return "disabled";
        }

        channel.ExchangeDeclare(message.ExchangeName, ExchangeType.Direct);
        channel.QueueDeclare(message.QueueName, true, false, false, null);
        channel.QueueBind(message.QueueName, message.ExchangeName, message.RoutingKey);

        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var receivedMessage = Encoding.UTF8.GetString(body);

            MessageData? response = JsonSerializer.Deserialize<MessageData>(receivedMessage);
            handler(response ?? new() { Data = "Err" });
        };

        return channel.BasicConsume(message.QueueName, true, consumer);
    }

    public void UnSubscribe(string tag)
    {
        if (isDisabled) return;
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
