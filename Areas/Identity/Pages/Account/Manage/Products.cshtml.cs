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

        public string Role { get; set; }
        public string ReturnUrl { get; set; }
        public IList<Product> Products { get; set; } = default!;

        public async Task OnGetAsync()
        {
            var userName = User.Identity.Name;
            var user = context.Users.Where(u => u.UserName == userName).FirstOrDefault();
            var roleId = context.UserRoles.Where(r => r.UserId == user.Id).FirstOrDefault();
            var role = context.Roles.Where(r => r.Id == roleId.RoleId).FirstOrDefault();
            Role = role.Name;
            if (context.Address != null)
            {
                if (role.Name == "Admin")
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