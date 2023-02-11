using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Karrot.Data;
using Karrot.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Karrot.Pages.CartItems
{
    public class OrderSummary : PageModel
    {
 public class RatingInputModel
        {
            [Required]
            public string Comment { get; set; }
            [Required]

            [Range(1, 5)]
            public int Rating { get; set; }
        }

                [BindProperty]
        public RatingInputModel Input { get; set; }

        private readonly ILogger<OrderSummary> _logger;
        private readonly KarrotDbContext _context;


        public OrderSummary(ILogger<OrderSummary> logger, KarrotDbContext context)
        {
            _logger = logger;
            _context = context;
        }


        public int Id { get; set; }

        public Order Order { get; set; }

        public async Task OnGet(int id)
        {
            Id = id;
            Order = await _context.Orders
                .Include(x => x.OrderItems).ThenInclude(x => x.Product).ThenInclude(x => x.Owner)
                .FirstAsync(x => x.OrderId == Id);
        }
    }
}