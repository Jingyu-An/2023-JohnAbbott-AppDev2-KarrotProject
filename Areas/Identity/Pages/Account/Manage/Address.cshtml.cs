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
    public class IndexModel : PageModel
    {
        private readonly KarrotDbContext context;

        public IndexModel(KarrotDbContext context)
        {
            this.context = context;
        }

        public IList<Address> Address { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (context.Address != null)
            {
                Address = await context.Address.Where(u => u.User.UserName == User.Identity.Name).ToListAsync();
            }
        }
    }
}
