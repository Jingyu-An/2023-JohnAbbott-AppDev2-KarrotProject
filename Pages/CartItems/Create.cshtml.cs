using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Karrot.Data;
using Karrot.Models;

namespace Karrot.Pages.CartItems
{
    public class CreateModel : PageModel
    {
        private readonly KarrotDbContext context;
        private ILogger<CreateModel> logger;

        public CreateModel(KarrotDbContext context, ILogger<CreateModel> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        [BindProperty(SupportsGet = true)] public int Id { get; set; }

        [BindProperty] public CartItem CartItem { get; set; } = default!;

        public Product Product { get; set; }

        public async Task<IActionResult> OnGet(int? id)
        {
            if (id == null || context.Products == null)
            {
                return NotFound();
            }

            var product = context.Products.Where(p => p.Id == id).FirstOrDefault();
            if (product == null)
            {
                return NotFound();
            }

            Product = product;

            await OnPostAsync(id);
            return RedirectToPage("./Index");
        }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync(int? id)
        {
            ModelState.Remove("CartItem.CartItemUser");
            ModelState.Remove("CartItem.CartItemProduct");
            if (!ModelState.IsValid || context.Products == null)
            {
                logger.LogInformation("Error");
                return Page();
            }

            var userName = User.Identity.Name;
            var user = context.Users.Where(u => u.UserName == userName).FirstOrDefault();
            var product = context.Products.Where(p => p.Id == Id).FirstOrDefault();

            var cartItem = context.CartItems.Where(c => c.CartItemProduct.Id == product.Id).FirstOrDefault();
            if (cartItem != null)
            {
                cartItem.CartQuantity++;
                try
                {
                    logger.LogInformation("Start add cart save");
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

                return RedirectToPage("./Index");
            }

            var newCartItem = new CartItem
            {
                CartQuantity = CartItem.CartQuantity, CartItemCreated = DateTime.Now,
                CartItemUser = user, CartItemProduct = product
            };

            context.CartItems.Add(newCartItem);
            await context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }

        private bool CartItemExists(int id)
        {
            return (context.CartItems?.Any(e => e.CartItemId == id)).GetValueOrDefault();
        }
    }
}