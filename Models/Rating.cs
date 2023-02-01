namespace Karrot.Models;

public class Rating
{
    public string RatingId { get; set; }

    public string UserId { get; set; }
    
    public int RatingValue { get; set; }
    
    public DateTime RatingCreated { get; set; }
}