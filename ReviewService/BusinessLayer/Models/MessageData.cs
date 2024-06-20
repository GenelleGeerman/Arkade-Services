namespace BusinessLayer.Models;

public class MessageData
{
    public long UserId { get; set; }
    public string ExchangeName { get; set; }
    public string RoutingKey { get; set; }
    public string Data { get; set; }
}

public static class MessageFactory
{
    public static MessageData GetProfileMessage(long userId)
    {
        return new()
        {
            UserId = userId,
            ExchangeName = "Profile",
            RoutingKey = "Profile"
        };
    }
}
