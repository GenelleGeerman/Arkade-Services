namespace BusinessLayer.Interfaces;

public interface IMessageService
{
    void Publish<T>(string exchangeName, string routingKey, T data);

    string Subscribe<T>(string exchangeName, string queueName, string routingKey, Action<T> handler);
    void UnSubscribe(string tag);
}
