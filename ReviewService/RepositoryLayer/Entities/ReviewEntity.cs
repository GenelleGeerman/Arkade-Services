using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BusinessLayer.Models;

namespace RepositoryLayer.Entities;

public class ReviewEntity
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int GameId { get; set; }
    
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public int UserId { get; set; }
    [Timestamp]
    public byte[] RowVersion { get; set; } = Array.Empty<byte>();
    
    public ReviewEntity() { }

    public ReviewEntity(Review review)
    {
        Title = review.Title;
        Description = review.Description;
        GameId = review.GameId;
        Id = review.Id;
        UserId = review.UserId;
    }
    public Review GetReview()
    {
        return new()
        {
            Id = Id,
            Title = Title,
            Description = Description,
            GameId = GameId,
            UserId = UserId
        };
    }

    public void Update(Review review)
    {
        if (!string.IsNullOrEmpty(review.Title)) Title = review.Title;
        if (!string.IsNullOrEmpty(review.Description)) Title = review.Description;
    }
}
