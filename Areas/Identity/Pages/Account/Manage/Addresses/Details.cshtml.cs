using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Karrot.Data;
using Karrot.Models;
using Microsoft.AspNetCore.Authorization;

namespace Karrot.Areas.Identity.Pages.Account.Manage.Addresses
{
    [Authorize]
    public class DetailsModel : PageModel
    {
        private readonly Karrot.Data.KarrotDbContext _context;

        public DetailsModel(Karrot.Data.KarrotDbContext context)
        {
            _context = context;
        }

      public Address Address { get; set; } = default!; 

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Address == null)
            {
                return NotFound();
            }

            var address = await _context.Address.FirstOrDefaultAsync(m => m.AddressId == id);
            if (address == null)
            {
                return NotFound();
            }
            else 
            {
                Address = address;
            }
            return Page();
        }
    }
}
