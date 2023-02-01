using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Karrot.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace Karrot.Data
{
    public class KarrotDbContext : IdentityDbContext
    {
        public KarrotDbContext(DbContextOptions<KarrotDbContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
    }
}