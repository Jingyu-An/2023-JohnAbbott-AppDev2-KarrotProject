using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Karrot.Data;
using Karrot.Models;

namespace Karrot.Pages.CartItems
{
    public class IndexModel : PageModel
    {
        private readonly KarrotDbContext context;
        private readonly ILogger<IndexModel> logger;

        public IndexModel(KarrotDbContext context, ILogger<IndexModel> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        [BindProperty]
        public int CartItemId { get; set; }
        
        [BindProperty]
        public int CartQuantity { get; set; }
        public IList<CartItem> CartItems { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (context.CartItems != null)
            {
                CartItems = await context.CartItems.Include("CartItemUser").Include("CartItemProduct").ToListAsync();
            }
        }
        
        public async Task<IActionResult> OnPostAsync()
        {
            ModelState.Remove("CartItem.CartItemUser");
            ModelState.Remove("CartItem.CartItemProduct");
            if (!ModelState.IsValid || context.Products == null)
            {
                logger.LogInformation("Error");
                return Page();
            }

            var cartItem = context.CartItems.Where(c => c.CartItemId == CartItemId).FirstOrDefault();
            if (cartItem != null)
            {
                cartItem.CartQuantity = CartQuantity;
                try
                {
                    await context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CartItemExists(cartItem.CartItemId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            return RedirectToPage("./Index");
        }
        private bool CartItemExists(int id)
        {
            return (context.CartItems?.Any(e => e.CartItemId == id)).GetValueOrDefault();
        }
    }
}
