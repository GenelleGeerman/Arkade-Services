using BusinessLayer.Models;

namespace BusinessLayer.Interfaces;

public interface IReviewService
{
    Task<Review> Create(Review request);

    Task<Review[]> GetByGameId(int gameId);

    Task<Review> Update(Review request);

    Task<bool> Delete(string token, int reviewId);

    Task<Review[]> GetByUserId(int userId);

    Task<Review> Get(int id);

    Task DeleteAllByUserId(long userId);
}
