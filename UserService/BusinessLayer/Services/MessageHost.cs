using BusinessLayer.Interfaces;
using BusinessLayer.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BusinessLayer.Services;

public class MessageHost : IHostedService
{
    private IServiceProvider serviceProvider;

    public Task StartAsync(CancellationToken cancellationToken)
    {
        var scope = serviceProvider.CreateScope();
        var messageService = serviceProvider.GetRequiredService<MessageService>();
        var profileService = serviceProvider.GetRequiredService<IProfileService>();
        messageService.Subscribe<MessageData>("Profile",
            "Profile", 
            "Profile",
            ProfileHandler());
        return Task.CompletedTask;
    }

    private Action<MessageData> ProfileHandler()
    {
        return async (message) => await HandleMessageAsync(message, async (logic, msg)
            => await logic.GetProfile(msg.UserId));
    }

    private async Task HandleMessageAsync(MessageData message, Func<IProfileService, MessageData, Task> action)
    {
        using (var scope = serviceProvider.CreateScope())
        {
            var userLogic = scope.ServiceProvider.GetRequiredService<ProfileService>();

            try
            {
                await action(userLogic, message);
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
        return Task.CompletedTask;
    }
}

public class MessageData
{
    public long UserId { get; set; }
    public string ExchangeName { get; set; }
    public string RoutingKey { get; set; }
    public string Data { get; set; }
}
