using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace BusinessLayer.Services;

public class MessageService
{
    private readonly IConfiguration configuration;
    private readonly IConnection connection;
    private IModel channel;

    public MessageService(IConfiguration configuration)
    {
        this.configuration = configuration;
        connection = Connect();
        channel = connection.CreateModel();
    }

    public IConnection Connect()
    {
        var factory = new ConnectionFactory
        {
            Uri = new Uri(configuration.GetConnectionString("RabbitMQContext"))
        };
        return factory.CreateConnection();
    }

    public void Publish<T>(string exchangeName, string routingKey, string queueName, T data, IBasicProperties props = null)
    {
        channel.ExchangeDeclare(exchange: exchangeName, type: ExchangeType.Direct);
        channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);
        var jsonMessage = JsonSerializer.Serialize(data);
        var body = Encoding.UTF8.GetBytes(jsonMessage);
        channel.BasicPublish(exchange: exchangeName, routingKey: routingKey,
            basicProperties: props, body: body);
    }

    public void Subscribe<T>(string exchangeName, string queueName, string routingKey, Action<T> handler)
    {
        channel.ExchangeDeclare(exchange: exchangeName, type: ExchangeType.Direct);
        channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);
        channel.QueueBind(queue: queueName, exchange: exchangeName, routingKey: routingKey);

        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var receivedMessage = Encoding.UTF8.GetString(body);
            Console.WriteLine(receivedMessage);

            // Deserialize the message using System.Text.Json
            var message = JsonSerializer.Deserialize<T>(receivedMessage);

            // Invoke the handler with the deserialized message
            handler(message);
        };

        channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);
    }

    public IBasicProperties CreateBasicProperties()
    {
        return channel.CreateBasicProperties();
    }
}
