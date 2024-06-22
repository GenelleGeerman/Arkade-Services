// ReviewService.cs

using BusinessLayer.Interfaces;
using BusinessLayer.Models;

namespace BusinessLayer.Services;

public class ReviewService(IReviewRepository repository, IMessageService msgService) : IReviewService
{
    public async Task<Review> Create(Review request)
    {
        Review response = await repository.Create(request);
        return response.Copy();
    }

    public async Task<Review[]> GetByGameId(int gameId)
    {
        return await repository.GetByGameId(gameId);
    }

    public async Task<Review> Update(Review review)
    {
        return await repository.Update(review);
    }

    public async Task<bool> Delete(int id)
    {
        return await repository.Delete(id);
    }

    public async Task<Review[]> GetByUserId(int id)
    {
        return await repository.GetByUserId(id);
    }

    public async Task<Review> Get(int id)
    {
        Review review = await repository.Get(id);
        int userId = review.UserId;

        Console.WriteLine("Sending Message");

        review.User = await GetUser(userId);
        return review;
    }

    public async Task DeleteAllByUserId(long userId)
    {
        await repository.DeleteAllByUserId(userId);
    }

    private async Task<string> GetUser(int userId)
    {
        var tcs = new TaskCompletionSource<MessageData>();

        string tag = msgService.Subscribe(MessageFactory.GetProfileResponse(),
            message => { tcs.SetResult(message); });
        msgService.Publish(MessageFactory.GetProfileMessage(userId));

        MessageData userMessage = await tcs.Task;
        msgService.UnSubscribe(tag);
        return userMessage.Data;
    }
}
