using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Karrot.Data;
using Karrot.Models;
using Microsoft.AspNetCore.Authorization;

namespace Karrot.Areas.Identity.Pages.Account.Manage.Addresses
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly KarrotDbContext context;

        public CreateModel(KarrotDbContext context)
        {
            this.context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public InputAddress Address { get; set; } = default!;
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
          if (!ModelState.IsValid || context.Address == null || Address == null)
            {
                return Page();
            }
            var userName = User.Identity.Name;
            var user = context.Users.Where(u => u.UserName == userName).FirstOrDefault();
            var addressLine2 = Address.AddressLine2 == null ? "-" : Address.AddressLine2;
            var address = new Address
            {
                AddressLine1 = Address.AddressLine1,
                AddressLine2 = addressLine2,
                City = Address.City,
                State = Address.State,
                Country = Address.Country,
                PostalCode = Address.PostalCode,
                User = user
            };

            context.Address.Add(address);
            await context.SaveChangesAsync();

            return RedirectToPage("../Address");
        }
        
        
        public class InputAddress
        {
            [Required]
            [StringLength(70)]
            public string AddressLine1 { get; set; }

            public string? AddressLine2 { get; set; }

            [Required]
            [StringLength(40)]
            public string City { get; set; }

            [Required]
            [StringLength(40)]
            public string State { get; set; }

            [Required]
            [Display(Name = "Postal Code")]
            [StringLength(10)]
            public string PostalCode { get; set; }

            [Required]
            [StringLength(40)]
            public string Country { get; set; }
        }
    }
}
