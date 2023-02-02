using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Karrot.Models;

public class CartItem
{
    [Key]
    public string CartItemId { get; set; }

    public IdentityUser CartItemUser { get; set; }

    public int CartQuantity { get; set; }

    public Product CartItemProduct { get; set; }
    
    public DateTime CartItemCreated { get; set; }
}