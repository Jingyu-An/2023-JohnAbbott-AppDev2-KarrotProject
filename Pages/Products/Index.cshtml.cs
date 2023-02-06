using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Karrot.Data;
using Karrot.Models;

namespace Karrot.Pages.Products
{
    public class IndexModel : PageModel
    {
        private readonly KarrotDbContext _context;

        public IndexModel(KarrotDbContext context)
        {
            _context = context;
        }

        public IList<Product> Product { get;set; } = default!;
        // public List<Product> Addresses { get;set; } = default!;
        // public List<Product> Category { get;set; } = default!;
        public async Task OnGetAsync()
        {
            if (_context.Products != null)
            {
                Product = await _context.Products.Include("Address").Include("Category").ToListAsync();
            }
        }
    }
}
