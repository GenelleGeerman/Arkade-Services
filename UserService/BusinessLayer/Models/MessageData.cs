namespace BusinessLayer.Models;

public class MessageData
{
    public long UserId { get; set; }
    public string ExchangeName { get; set; }
    public string RoutingKey { get; set; }
    public string QueueName { get; set; }
    public string Data { get; set; }
}

public class MessageFactory
{
    public static MessageData GetDeleteMessage(long userId)
    {
        return new()
        {
            UserId = userId,
            ExchangeName = "DeleteUser",
            RoutingKey = "DeleteUser",
            QueueName = "DeleteUser"
        };
    }
}
