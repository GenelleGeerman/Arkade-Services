using System.Text.Json;
using BusinessLayer.Interfaces;
using BusinessLayer.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BusinessLayer.Services;

public class MessageHost(IServiceProvider serviceProvider) : IHostedService
{
    public Task StartAsync(CancellationToken cancellationToken)
    {
        var scope = serviceProvider.CreateScope();
        var messageService = scope.ServiceProvider.GetRequiredService<IMessageService>();
        var profileService = scope.ServiceProvider.GetRequiredService<IProfileService>();
        MessageData sub = MessageFactory.GetProfileMessage();
        messageService.Subscribe(sub,
            async (message) => await HandleProfile(messageService, message));
        return Task.CompletedTask;
    }

    private async Task HandleProfile(IMessageService messageService, MessageData message)
    {
        using (IServiceScope scope = serviceProvider.CreateScope())
        {
            IProfileService profileService = scope.ServiceProvider.GetRequiredService<IProfileService>();

            try
            {
                Console.WriteLine("Processing profile message");
                var user = await profileService.GetProfile(message.UserId);
                MessageData data = new()
                {
                    ExchangeName = "ProfileResponse",
                    RoutingKey = "ProfileResponse",
                    QueueName = "ProfileResponse",
                    Data = JsonSerializer.Serialize(user)
                };
                messageService.Publish(data);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error processing message");
                Console.WriteLine(ex.Message);
            }
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        // Optionally, add code here to unsubscribe from messages and clean up resources if necessary.
        return Task.CompletedTask;
    }
}
