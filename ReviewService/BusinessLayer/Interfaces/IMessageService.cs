using BusinessLayer.Models;

namespace BusinessLayer.Interfaces;

public interface IMessageService
{
    void Publish(MessageData data);

    string Subscribe(MessageData message, Action<MessageData> handler);

    void UnSubscribe(string tag);
}
