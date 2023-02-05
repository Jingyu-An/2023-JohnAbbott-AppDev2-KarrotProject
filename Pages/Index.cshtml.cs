using Karrot.Data;
using Karrot.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Karrot.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly KarrotDbContext _context;
    public IList<Product> Product { get;set; } = default!;
    public List<Product> Addresses { get;set; } = default!;
    public IndexModel(ILogger<IndexModel> logger, KarrotDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task OnGetAsync()
    {
        if (_context.Products != null)
        {
            Product = await _context.Products.ToListAsync();
            Addresses = await _context.Products.Include("Address").ToListAsync();
        }
    }
}
