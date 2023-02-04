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
using Microsoft.AspNetCore.Authorization;

namespace Karrot.Areas.Identity.Pages.Account.Manage.Addresses
{
    [Authorize]
    public class EditModel : PageModel
    {
        private readonly KarrotDbContext context;

        public EditModel(KarrotDbContext context)
        {
            this.context = context;
        }
        
        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        [BindProperty]
        public Address Address { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || context.Address == null)
            {
                return NotFound();
            }

            var address =  await context.Address.FirstOrDefaultAsync(m => m.AddressId == id);
            if (address == null)
            {
                return NotFound();
            }
            Address = address;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            var address = await context.Address.FindAsync(Id);
            ModelState.Remove("Address.User");
            
            if (!ModelState.IsValid)
            {
                return Page();
            }

            address.AddressLine1 = Address.AddressLine1;
            address.AddressLine2 = Address.AddressLine2;
            address.City = Address.City;
            address.State = Address.State;
            address.Country = Address.Country;
            address.PostalCode = address.PostalCode;

            context.Address.Update(address);
            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AddressExists(Address.AddressId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("../Address");
        }

        private bool AddressExists(int id)
        {
          return (context.Address?.Any(e => e.AddressId == id)).GetValueOrDefault();
        }
    }
}
