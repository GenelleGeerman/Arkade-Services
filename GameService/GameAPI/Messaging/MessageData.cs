namespace GameAPI.Messaging;

public class MessageData
{
    public int GameId { get; set; }
    public string ExchangeName { get; set; } = string.Empty;
    public string RoutingKey { get; set; } = string.Empty;
    public string QueueName { get; set; } = string.Empty;
    public string Data { get; set; } = string.Empty;
}

public static class MessageFactory
{
   

    public static MessageData GetGameIdMessage()
    {
        return new()
        {
            ExchangeName = "GameId",
            RoutingKey = "GameId",
            QueueName = "GameId"
        };
    }

    public static MessageData GetGameIdResponse()
    {
        return new()
        {
            ExchangeName = "GameIdResponse",
            RoutingKey = "GameIdResponse",
            QueueName = "GameIdResponse"
        };
    }
}
