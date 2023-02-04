using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Karrot.Models;

public class Address
{
    [Key]
    public int AddressId { get; set; }
    
    [Required(ErrorMessage = "Address is required")]
    [StringLength(70)]
    public string AddressLine1 { get; set; }
    
    public string AddressLine2 { get; set; }

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
    
    public IdentityUser User { get; set; }

    public override string ToString()
    {
        if (AddressLine2 == "-")
        {
            AddressLine2 = "";
        }
        else
        {
            AddressLine2 = ", " + AddressLine2;
        }
        return $"{AddressLine1}{AddressLine2}, {City}, {State}, {Country}, {PostalCode}";
    }
}