namespace BusinessLayer.Models;

public class MessageData
{
    public long UserId { get; set; }
    public string ExchangeName { get; set; } = string.Empty;
    public string RoutingKey { get; set; } = string.Empty;
    public string QueueName { get; set; } = string.Empty;
    public string Data { get; set; } = string.Empty;
}

public static class MessageFactory
{
    public static MessageData GetProfileMessage(long userId)
    {
        return new()
        {
            UserId = userId,
            ExchangeName = "Profile",
            RoutingKey = "Profile",
            QueueName = "Profile"
        };
    }

    public static MessageData GetDeleteUserMessage()
    {
        return new()
        {
            ExchangeName = "DeleteUser",
            RoutingKey = "DeleteUser",
            QueueName = "DeleteUser"
        };
    }

    public static MessageData GetProfileResponse()
    {
        return new()
        {
            ExchangeName = "ProfileResponse",
            RoutingKey = "ProfileResponse",
            QueueName = "ProfileResponse"
        };
    }
}
