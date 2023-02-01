using System.ComponentModel.DataAnnotations;

namespace Karrot.Models;

public class Category
{
    [Key]
    public int CategoryId { get; set; }
    [Required, StringLength(100), Display(Name = "Name")]
    public string CategoryName { get; set; }
}