using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Karrot.Models;

public class Order
{
    [Key]
    public string OrderId { get; set; }

    public DateTime OrderDate { get; set; }
    
    public IdentityUser OrderUser { get; set; }
    
    [Required(ErrorMessage = "First Name is required")]
    [Display(Name = "First Name")]
    [StringLength(160)]
    public string FirstName { get; set; }

    [Required(ErrorMessage = "Last Name is required")]
    [Display(Name = "Last Name")]
    [StringLength(160)]
    public string LastName { get; set; }

    [Required(ErrorMessage = "Address is required")]
    [StringLength(70)]
    public string Address { get; set; }

    [Required(ErrorMessage = "City is required")]
    [StringLength(40)]
    public string City { get; set; }

    [Required(ErrorMessage = "State is required")]
    [StringLength(40)]
    public string State { get; set; }

    [Required(ErrorMessage = "Postal Code is required")]
    [Display(Name = "Postal Code")]
    [StringLength(10)]
    public string PostalCode { get; set; }

    [Required(ErrorMessage = "Country is required")]
    [StringLength(40)]
    public string Country { get; set; }

    [StringLength(24)]
    public string Phone { get; set; }

    [Required(ErrorMessage = "Email Address is required")]
    [Display(Name = "Email Address")]
    [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}",
        ErrorMessage = "Email is is not valid.")]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; }
    
    public int Total { get; set; }
    
    public string PaymentTransactionId { get; set; }

    public List<OrderItem> OrderItems { get; set; }
}