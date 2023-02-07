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
    public class CommentsModel : PageModel
    {
        private readonly KarrotDbContext context;
        private readonly ILogger<CommentsModel> logger;

        public CommentsModel(KarrotDbContext context, ILogger<CommentsModel> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        public string Role { get; set; }
        public IList<Comment> Comments { get; set; } = default!;

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
                    Comments = await context.Comments.Include("CommentUser").Include("CommentProduct").ToListAsync();
                }
                else
                {
                    Comments = await context.Comments.Include("CommentProduct")
                        .Where(u => u.CommentUser.UserName == User.Identity.Name).ToListAsync();
                }
            }
        }
    }
}