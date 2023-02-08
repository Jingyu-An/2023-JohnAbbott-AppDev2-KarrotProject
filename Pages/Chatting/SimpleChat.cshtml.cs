using Karrot.Data;
using Karrot.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Karrot.Pages.Chatting;

public class SimpleChat : PageModel
{
    private readonly KarrotDbContext _context;
  
    public SimpleChat(KarrotDbContext context)
    {
        _context = context;
    }
    public Product Product { get; set; } = default!;
    public string Image { get; set; }
    public string Address { get; set; }

    [BindProperty(SupportsGet = true)] 
    public int Id { get; set; }
    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null || _context.Products == null)
        {
            return NotFound();
        }

        var product = await _context.Products.Include("Owner").Include("Address")
            .FirstOrDefaultAsync(m => m.Id == id);
        
        if (product == null)
        {
            return NotFound();
        }
        else
        {
            Product = product;
        }
        Image = product.Image;

        Address = product.Address.ToString();
        return Page();
    }
}