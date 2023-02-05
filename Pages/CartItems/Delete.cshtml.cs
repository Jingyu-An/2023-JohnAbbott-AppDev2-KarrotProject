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
    public class DeleteModel : PageModel
    {
        private readonly KarrotDbContext context;
        private ILogger<DeleteModel> logger;

        public DeleteModel(KarrotDbContext context, ILogger<DeleteModel> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        [BindProperty] public CartItem CartItem { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || context.CartItems == null)
            {
                return NotFound();
            }

            var cartitem = await context.CartItems.FirstOrDefaultAsync(m => m.CartItemId == id);

            if (cartitem == null)
            {
                return NotFound();
            }
            else
            {
                CartItem = cartitem;
            }

            await OnPostAsync(id);
            return RedirectToPage("./Index");
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || context.CartItems == null)
            {
                return NotFound();
            }

            var cartItem = await context.CartItems.FindAsync(id);

            if (cartItem != null)
            {
                CartItem = cartItem;
                context.CartItems.Remove(CartItem);
                await context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}