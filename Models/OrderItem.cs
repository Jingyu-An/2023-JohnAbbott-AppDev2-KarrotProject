using Microsoft.AspNetCore.Identity;

namespace Karrot.Models;

public class OrderItem
{
    public int OrderItemId { get; set; }
    //public string OrderId { get; set; }
    public Order Order { get; set; }
    
    public IdentityUser OrderItemUser { get; set; }

    public int OrderQuantity { get; set; }

    // public int ProductId { get; set; }
    public Product Product { get; set; }
    
    public double Price { get; set; }
}