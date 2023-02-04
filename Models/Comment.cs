using Microsoft.AspNetCore.Identity;

namespace Karrot.Models;

public class Comment 
{
    public int CommentId { get; set; }

    public IdentityUser CommentUser { get; set; }
    
    public Product CommentProduct { get; set; }
    
    public string CommentBody { get; set; }
    
    public string CommentTitle { get; set; }
    
    public DateTime CommentCreated { get; set; }
   
}