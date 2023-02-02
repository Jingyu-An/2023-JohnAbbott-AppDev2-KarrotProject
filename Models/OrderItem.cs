using Microsoft.AspNetCore.Identity;

namespace Karrot.Models;

public class OrderItem
{
    public string OrderItemId { get; set; }

    public string OrderId { get; set; }
    
    public IdentityUser OrderItemUser { get; set; }

    public int OrderQuantity { get; set; }

    public int ProductId { get; set; }
    
    public double Price { get; set; }
}