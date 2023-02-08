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
using Microsoft.AspNetCore.Identity;

namespace Karrot.Areas.Identity.Pages.Account.Manage
{
    [Authorize(Roles = "Admin")]
    public class UsersModel : PageModel
    {
        private readonly KarrotDbContext context;
        private readonly ILogger<UsersModel> logger;

        public UsersModel(KarrotDbContext context,
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            ILogger<UsersModel> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        [BindProperty]
        public IList<IdentityUser> Users { get; set; } = default!;

        public async Task OnGetAsync()
        {
            if (context.Address != null)
            {
                Users = await context.Users.ToListAsync();
            }
        }
    }
}