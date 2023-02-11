using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Karrot.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace Karrot.Pages.CartItems
{

    public class Rating : PageModel
    {
        private readonly KarrotDbContext context;
        public class RatingInputModel
        {
            [Required]
            public string Comment { get; set; }
            [Required]

            [Range(1, 5)]
            public int Rating { get; set; }
        }

        private readonly ILogger<Rating> _logger;

        public Rating(ILogger<Rating> logger, KarrotDbContext context)
        {
            _logger = logger;
            this.context = context;
        }

        [BindProperty]
        public RatingInputModel Input { get; set; }

        public int OrderId { get; set; }
        public string SellerId { get; set; }


        public void OnGet([FromQuery] int orderId, [FromQuery] string sellerId)
        {
            OrderId = orderId;
            SellerId = sellerId;
        }

        public async Task<IActionResult> OnPostAsync([FromQuery] int orderId, [FromQuery] string sellerId)
        {
            var userName = User.Identity.Name;
            var user = context.Users.Where(u => u.UserName == userName).FirstOrDefault();

            var seller = context.Users.Where(u => u.Id == sellerId).FirstOrDefault();



            var rating = new Models.Rating();

            rating.RatingUser = user;
            rating.RatingCreated = DateTime.Now;
            rating.RatingValue = Input.Rating;
            rating.RatedSeller = seller;

            context.Ratings.Add(rating);

            await context.SaveChangesAsync();
            return RedirectToPage("OrderSummary", new { Id = orderId });
        }
    }
}