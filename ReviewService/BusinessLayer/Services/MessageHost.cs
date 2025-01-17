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
        var messageService = scope.ServiceProvider.GetRequiredService<MessageService>();
        MessageData data = MessageFactory.GetDeleteUserMessage();
        messageService.Subscribe(data,
            async message => await HandleDeletion(message));
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        // Optionally, add code here to unsubscribe from messages and clean up resources if necessary.
        return Task.CompletedTask;
    }

    private async Task HandleDeletion(MessageData message)
    {
        using (IServiceScope scope = serviceProvider.CreateScope())
        {
            IReviewService reviewService = scope.ServiceProvider.GetRequiredService<IReviewService>();

            try
            {
                reviewService.DeleteAllByUserId(message.UserId);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error processing message");
                Console.WriteLine(ex.Message);
            }
        }
    }
}
