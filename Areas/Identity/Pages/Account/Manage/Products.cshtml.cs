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

namespace Karrot.Areas.Identity.Pages.Account.Manage
{
    [Authorize]
    public class ProductsModel : PageModel
    {
        private readonly KarrotDbContext context;

        public ProductsModel(KarrotDbContext context)
        {
            this.context = context;
        }

        public string ReturnUrl { get; set; }
        public IList<Product> Products { get; set; } = default!;

        public async Task OnGetAsync()
        {
            if (context.Address != null)
            {
                if (User.IsInRole("Admin"))
                {
                    Products = await context.Products.Include("Owner").ToListAsync();
                }
                else
                {
                    Products = await context.Products.Where(u => u.Owner.UserName == User.Identity.Name).ToListAsync();
                }
            }

            ReturnUrl = "/Account/Manage/Products";
        }
    }
}