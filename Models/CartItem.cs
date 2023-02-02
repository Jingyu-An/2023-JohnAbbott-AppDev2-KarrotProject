using System.ComponentModel.DataAnnotations;

namespace Karrot.Models;

public class CartItem
{
    [Key]
    public string CartItemId { get; set; }

    public string UserId { get; set; }

    public int CartQuantity { get; set; }

    public int ProductId { get; set; }
    
    public DateTime CartItemCreated { get; set; }
}