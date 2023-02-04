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

namespace Karrot.Pages.Products
{
    [Authorize]
    public class DetailsModel : PageModel
    {
        private readonly KarrotDbContext _context;

        public DetailsModel(KarrotDbContext context)
        {
            _context = context;
        }

      public Product Product { get; set; } = default!; 
      [BindProperty]
      public Comment Comment { get; set; } = default!;
      public List<Comment>? Comments { get; set; }
      
      [BindProperty(SupportsGet = true)]
      public int Id { get; set; }
      
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products.Include("Address").Include("Category")
                .FirstOrDefaultAsync(m => m.Id == id);
            
            
            if (product == null)
            {
                return NotFound();
            }
            else 
            {
                Product = product;
            }

           
            
            return Page();
        }
        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {

            var userName = User.Identity.Name;
            var user = _context.Users.Where(u => u.UserName == userName).FirstOrDefault();
         
            var commentProductId = Id;
        
            if (User.Identity.IsAuthenticated)
            {
                Comment.CommentUser = user;
                Comment.CommentProduct = await _context.Products.FindAsync(commentProductId);
                Comment.CommentCreated = DateTime.Now;
                Comment.CommentBody = Comment.CommentBody;
                Comment.CommentTitle = "Title";
            }

            _context.Comments.Add(Comment);
            await _context.SaveChangesAsync();
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
