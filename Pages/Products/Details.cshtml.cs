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
using Rating = Karrot.Pages.CartItems.Rating;

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
        public Rating Rating { get; set; }
        public List<Comment>? Comments { get; set; }
        public List<Models.Rating> Ratings { get; set; }
        public int ratingValue { get; set; }
        public double ratingAvg { get; set; }
        public string Image { get; set; }
        public string CategoryName { get; set; }
        public string Address { get; set; }

        [BindProperty(SupportsGet = true)] 
        public int Id { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products.Include("Owner").Include("Address").Include("Category")
                .FirstOrDefaultAsync(m => m.Id == id);

            var ratingSeller = await _context.Ratings.Include("RatedSeller")
                .FirstOrDefaultAsync(r => r.RatedSeller.Id == product.Owner.Id);
            
            if (ratingSeller == null)
            {
                ratingValue = 0;
            }
            else
            {
                ratingValue = ratingSeller.RatingValue;
            }


            Comments = await _context.Comments.Include("CommentUser").Include("CommentProduct").Where(c => c.CommentProduct.Id == Id).ToListAsync();
            Ratings = await _context.Ratings.Include("RatedSeller").Where(r => r.RatedSeller.Id == product.Owner.Id)
                .ToListAsync();

            if (Ratings == null)
            {
                ratingAvg = 0;
            }
            else
            {
                foreach (var sellerRate in Ratings)
                {
                    ratingValue = ratingValue + sellerRate.RatingValue;
                }
                ratingAvg = ratingValue / (double)Ratings.Count;
            }

            
            
            if (product == null)
            {
                return NotFound();
            }
            else
            {
                Product = product;
            }

            Image = product.Image;
            CategoryName = product.Category.CategoryName;
            Address = product.Address.ToString();

            return Page();
        }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            // Comments = await _context.Comments.Where(c => c.CommentProduct.Id == Id)
            //     .Include("Comment.CommentUser.UserName").ToListAsync();
            //
            ModelState.Remove("Comment.CommentUser");
            ModelState.Remove("Comment.CommentProduct");
            ModelState.Remove("Comment.CommentTitle");
            //
            if (!ModelState.IsValid || _context.Comments == null || Comment == null)
            {
                return RedirectToPage("./Details", new { Id = Id });
            }

            var userName = User.Identity.Name;
            var user = _context.Users.Where(u => u.UserName == userName).FirstOrDefault();

            var commentProductId = Id;

            Comment.CommentUser = user;
            Comment.CommentProduct = await _context.Products.FindAsync(commentProductId);
            Comment.CommentCreated = DateTime.Now;
            Comment.CommentBody = Comment.CommentBody;
            Comment.CommentTitle = "Title";

            _context.Comments.Add(Comment);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Details", new { Id = Id });
        }

        public IActionResult Tests()
        {
            return RedirectToPage("./Index");
        }
    }
}