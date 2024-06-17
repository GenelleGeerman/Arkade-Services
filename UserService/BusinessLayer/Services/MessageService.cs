using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace BusinessLayer.Services;

public class MessageService
{
    private readonly IConfiguration _configuration;
    private readonly IConnection _connection;
    private IModel _channel;

    public MessageService(IConfiguration configuration)
    {
        _configuration = configuration;
        _connection = Connect();
        _channel = _connection.CreateModel();
    }

    public IConnection Connect()
    {
        var factory = new ConnectionFactory
        {
            Uri = new Uri(_configuration.GetConnectionString("RabbitMQContext"))
        };
        return factory.CreateConnection();
    }

    public void Publish<T>(string exchangeName, string routingKey, T data, IBasicProperties props = null)
    {
        _channel.ExchangeDeclare(exchange: exchangeName, type: ExchangeType.Direct);
        var jsonMessage = JsonSerializer.Serialize(data);
        var body = Encoding.UTF8.GetBytes(jsonMessage);
        _channel.BasicPublish(exchange: exchangeName, routingKey: routingKey,
            basicProperties: props, body: body);
    }

    public void Subscribe<T>(string exchangeName, string queueName, string routingKey, Action<T> handler)
    {
        _channel.ExchangeDeclare(exchange: exchangeName, type: ExchangeType.Direct);
        _channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);
        _channel.QueueBind(queue: queueName, exchange: exchangeName, routingKey: routingKey);

        var consumer = new EventingBasicConsumer(_channel);
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

        _channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);
    }

    public IBasicProperties CreateBasicProperties()
    {
        return _channel.CreateBasicProperties();
    }
}
