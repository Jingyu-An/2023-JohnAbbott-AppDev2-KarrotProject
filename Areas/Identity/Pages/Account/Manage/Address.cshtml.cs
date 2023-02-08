
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Karrot.Models;
using Karrot.Data;
using Microsoft.EntityFrameworkCore;

namespace Karrot.Areas.Identity.Pages.Account.Manage.Addresses
{
    [Authorize]
    public class AddressModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ILogger<ChangePasswordModel> _logger;
        private readonly KarrotDbContext _context;


        public AddressModel(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            KarrotDbContext context,
        ILogger<ChangePasswordModel> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _context = context;
        }
        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            public string Address { get; set; }

            [Required]
            public string City { get; set; }

            [Required]
            public string State { get; set; }

            [Required]
            public string PostalCode { get; set; }

        }

        public async Task<IActionResult> OnGetAsync()
        {
            var userId = _userManager.GetUserId(User);
            var address = await _context.Address.FirstOrDefaultAsync(x => x.User.Id == userId);


            Input = new InputModel
            {
                Address = address.AddressLine1,
                City = address.City,
                State = address.State,
                PostalCode = address.PostalCode
            };
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var userId = _userManager.GetUserId(User);
            var address = await _context.Address.FirstOrDefaultAsync(x => x.User.Id == userId);

            address.AddressLine1 = Input.Address;
            address.City = Input.City;
            address.State = Input.State;
            address.PostalCode = Input.PostalCode;

            await _context.SaveChangesAsync();
            return Page();
        }


    }
}
