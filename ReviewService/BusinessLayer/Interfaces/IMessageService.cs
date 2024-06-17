namespace BusinessLayer.Interfaces;

public interface IMessageService
{
    void Publish<T>(string exchangeName, string routingKey, T data);

    void Subscribe<T>(string exchangeName, string queueName, string routingKey, Action<T> handler);
}
