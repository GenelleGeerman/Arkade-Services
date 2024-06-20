using BusinessLayer.Models;

namespace BusinessLayer.Interfaces;

public interface IReviewRepository
{
    Task<Review> Create(Review review);

    Task<Review> Get(int id);

    Task<Review> Update(Review review);

    Task<bool> Delete(int id);

    Task<Review[]> GetByGameId(int gameId);

    Task<Review[]> GetByUserId(int id);

    Task DeleteAllByUserId(long userId);
}
