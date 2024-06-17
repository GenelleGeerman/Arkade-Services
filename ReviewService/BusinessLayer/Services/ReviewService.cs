// ReviewService.cs
using BusinessLayer.Interfaces;
using BusinessLayer.Models;
using System.Text.Json;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
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

            var userId = review.UserId;

            var tcs = new TaskCompletionSource<string>();

            // Subscribe to the response queue
            msgService.Subscribe<string>("UserExchange", "reviewServiceQueue", $"user.response.{userId}", jsonResponse =>
            {
                tcs.SetResult(jsonResponse);
            });

            // Publish the request to the user service
            msgService.Publish("UserExchange", "user.request", new { UserId = userId });

            // Wait for the user service to respond
            var userJsonResponse = await tcs.Task;
            review.User = userJsonResponse;
            return review;
        }
    }
}
