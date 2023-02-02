namespace Karrot.Models;

public class Comment
{
    public string CommentId { get; set; }

    public string UserId { get; set; }
    
    public string CommentBody { get; set; }
    
    public string CommentTitle { get; set; }
    
    public DateTime CommentCreated { get; set; }
}