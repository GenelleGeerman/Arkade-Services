namespace BusinessLayer.Models;

public class Review
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int GameId { get; set; }
    public int Id { get; set; }
    public int UserId { get; set; }
    public string User { get; set; } = string.Empty;

    public Review Copy()
    {
        return (Review)MemberwiseClone();
    }
}
