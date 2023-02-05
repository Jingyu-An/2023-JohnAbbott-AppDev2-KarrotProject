using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Karrot.Pages.Products;

public class CommentDelete : PageModel
{
    private readonly Karrot.Data.KarrotDbContext _context;

    public CommentDelete(Karrot.Data.KarrotDbContext context)
    {
        _context = context;
    }

    [BindProperty]
    public Models.Comment Comment { get; set; } = default!;

    public Models.Product Product { get; set; } = default!;
    
    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null || _context.Comments == null)
        {
            return NotFound();
        }

        var comment = await _context.Comments.FirstOrDefaultAsync(m => m.CommentId == id);

        if (comment == null)
        {
            return NotFound();
        }
        else
        {
            Comment = comment;
        }

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int? id)
    {
        if (id == null || _context.Comments == null)
        {
            return NotFound();
        }

        var comment = await _context.Comments.FindAsync(id);
        
        var product = await _context.Comments.Include("CommentProduct")
            .FirstOrDefaultAsync(m => m.CommentId == id);
     
        if (comment != null)
        {
            Comment = comment;
            _context.Comments.Remove(Comment);
            await _context.SaveChangesAsync();
          
        }
        

        return RedirectToPage("./Details", new { Id = product.CommentProduct.Id});
    }
}