using Karrot.Data;
using Karrot.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Karrot.Pages.CartItems;

public class DeleteAll : PageModel
{
    private readonly KarrotDbContext context;
    private ILogger<DeleteAll> logger;

    public DeleteAll(KarrotDbContext context, ILogger<DeleteAll> logger)
    {
        this.context = context;
        this.logger = logger;
    }

    public IList<CartItem> CartItems { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync()
    {
        await OnPostAsync();
        return RedirectToPage("./Index");
    }


    public async Task<IActionResult> OnPostAsync()
    {
        if (context.CartItems == null)
        {
            return NotFound();
        }

        CartItems = await context.CartItems.Where(u => u.CartItemUser.UserName == User.Identity.Name).ToListAsync();

        if (CartItems != null)
        {
            foreach (var cartItem in CartItems)
            {
                context.CartItems.Remove(cartItem);
            }
            await context.SaveChangesAsync();
        }
        return RedirectToPage("./Index");
    }
}