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

        public IList<Comment> Comments { get; set; } = default!;

        public async Task OnGetAsync()
        {
            if (context.Address != null)
            {
                if (User.IsInRole("Admin"))
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