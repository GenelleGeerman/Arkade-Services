using BusinessLayer.Interfaces;
using BusinessLayer.Models;
using Microsoft.EntityFrameworkCore;
using RepositoryLayer.Entities;

namespace RepositoryLayer;

public class ReviewRepository : IReviewRepository
{
    public ReviewRepository(ReviewContext context)
    {
        this.context = context ?? throw new ArgumentNullException(nameof(context));
    }

    private ReviewContext context { get; }

    public async Task<Review> Create(Review request)
    {
        context.Set<ReviewEntity>().Add(new(request));
        return await context.SaveChangesAsync() == 1 ? request : new();
    }

    public async Task<Review> Get(int id)
    {
        ReviewEntity? entity = await context.Set<ReviewEntity>().FindAsync(id);
        return entity == null ? new() : entity.GetReview();
    }

    public async Task<Review> Update(Review request)
    {
        ReviewEntity entity = new(request);
        context.Entry(entity).State = EntityState.Modified;
        await context.SaveChangesAsync();
        return entity.GetReview();
    }

    public async Task<Review[]> GetByGameId(int gameId)
    {
        return await context.Reviews
            .Where(r => r.GameId == gameId)
            .Select(r => r.GetReview())
            .ToArrayAsync();
    }

    public async Task<Review[]> GetByUserId(int id)
    {
        return await context.Reviews
            .Where(r => r.UserId == id)
            .Select(r => r.GetReview())
            .ToArrayAsync();
    }

    public Task DeleteAllByUserId(long userId)
    {
        var entities = context.Reviews.Where(c => c.UserId == userId).ToList();
        context.Reviews.RemoveRange(entities);
        context.SaveChanges();
        return Task.CompletedTask;
    }

    public async Task<bool> Delete(int userId, int reviewId)
    {
        ReviewEntity? entity = context.Reviews.FirstOrDefault(e => e.Id == reviewId);
        if (entity == null) return false;
        if (entity.UserId != userId) return false;
        context.Set<ReviewEntity>().Remove(entity);
        return await context.SaveChangesAsync() > 0;
    }
}
