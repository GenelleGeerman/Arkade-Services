using System.Text.Json;
using GameAPI.Steam;

namespace GameAPI.Messaging;

public class MessageHost(IServiceProvider serviceProvider) : IHostedService
{
    public Task StartAsync(CancellationToken cancellationToken)
    {
        var scope = serviceProvider.CreateScope();
        var messageService = scope.ServiceProvider.GetRequiredService<MessageService>();
        MessageData data = MessageFactory.GetGameIdMessage();
        messageService.Subscribe(data, async message => await HandleGetById(messageService, message));
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        // Optionally, add code here to unsubscribe from messages and clean up resources if necessary.
        return Task.CompletedTask;
    }

    private async Task HandleGetById(MessageService messageService, MessageData message)
    {
        using (IServiceScope scope = serviceProvider.CreateScope())
        {
            SteamApi steamApi = scope.ServiceProvider.GetRequiredService<SteamApi>();

            try
            {
                SteamGame game = await steamApi.GetById(message.GameId);
                string gameJson = JsonSerializer.Serialize(game);
                MessageData reply = MessageFactory.GetGameIdResponse();
                reply.Data = gameJson;
                messageService.Publish(reply);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error processing message");
                Console.WriteLine(ex.Message);
            }
        }
    }
}
