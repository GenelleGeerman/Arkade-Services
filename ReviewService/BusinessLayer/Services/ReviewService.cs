// ReviewService.cs

using BusinessLayer.Interfaces;
using BusinessLayer.Models;

namespace BusinessLayer.Services;

public class ReviewService(IReviewRepository repository, IMessageService msgService, IAuthorizationService authService)
    : IReviewService
{
    public async Task<Review[]> GetByGameId(int gameId)
    {
        return await repository.GetByGameId(gameId);
    }

    public async Task<Review> Create(Review request)
    {
        int id = authService.GetUserId(request.Token);
        if (request.Id != id) request.Id = id;
        Review response = await repository.Create(request);
        return response.Copy();
    }

    public async Task<Review> Update(Review request)
    {
        int id = authService.GetUserId(request.Token);
        if (request.Id != id) request.Id = id;
        return await repository.Update(request);
    }

    public async Task<bool> Delete(string token, int reviewId)
    {
        int userId = authService.GetUserId(token);
        return await repository.Delete(userId, reviewId);
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
