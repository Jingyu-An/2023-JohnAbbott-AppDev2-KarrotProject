using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Karrot.Data;
using Karrot.Models;

namespace Karrot.Pages.CartItems
{
    public class EditModel : PageModel
    {
        private readonly Karrot.Data.KarrotDbContext _context;

        public EditModel(Karrot.Data.KarrotDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public CartItem CartItem { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.CartItems == null)
            {
                return NotFound();
            }

            var cartitem =  await _context.CartItems.FirstOrDefaultAsync(m => m.CartItemId == id);
            if (cartitem == null)
            {
                return NotFound();
            }
            CartItem = cartitem;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(CartItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CartItemExists(CartItem.CartItemId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool CartItemExists(int id)
        {
          return (_context.CartItems?.Any(e => e.CartItemId == id)).GetValueOrDefault();
        }
    }
}
