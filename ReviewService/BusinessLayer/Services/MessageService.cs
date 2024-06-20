using System.Text;
using System.Text.Json;
using BusinessLayer.Interfaces;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace BusinessLayer.Services;

public class MessageService : IMessageService
{
    private readonly IConfiguration configuration;
    private readonly IModel channel;

    public MessageService(IConfiguration configuration)
    {
        this.configuration = configuration;
        IConnection connection = Connect();
        channel = connection.CreateModel();
        Console.WriteLine("Messaging Online!");
    }

    private IConnection Connect()
    {
        var factory = new ConnectionFactory
        {
            Uri = new(configuration.GetConnectionString("RabbitMQContext") ?? string.Empty)
        };

        return factory.CreateConnection();
    }

    public void Publish<T>(string exchangeName, string routingKey, T data)
    {
        channel.ExchangeDeclare(exchangeName, ExchangeType.Direct);
        var jsonMessage = JsonSerializer.Serialize(data);
        var body = Encoding.UTF8.GetBytes(jsonMessage);
        channel.BasicPublish(exchangeName, routingKey, null, body);
    }

    public string Subscribe<T>(string exchangeName, string queueName, string routingKey, Action<T> handler)
    {
        channel.ExchangeDeclare(exchangeName, ExchangeType.Direct);
        channel.QueueDeclare(queueName, true, false, false, null);
        channel.QueueBind(queueName, exchangeName, routingKey);

        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var receivedMessage = Encoding.UTF8.GetString(body);
            Console.WriteLine(receivedMessage);

            var message = JsonSerializer.Deserialize<T>(receivedMessage);
            handler(message);
        };

        return channel.BasicConsume(queueName, true, consumer);
    }

    public void UnSubscribe(string tag)
    {
        channel.BasicCancel(tag);
    }
}
