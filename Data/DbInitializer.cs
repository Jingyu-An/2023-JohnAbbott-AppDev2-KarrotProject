using Karrot.Models;
using Microsoft.EntityFrameworkCore;

namespace Karrot.Data;

public static class DbInitializer
{
    public static void Initialize(KarrotDbContext karrotDbContext)
    {
        if (!karrotDbContext.Categories.Any())
        {
            var audio = new Category { CategoryName = "Audio" };
            var baby = new Category { CategoryName = "Baby" };
            var beauty = new Category { CategoryName = "Beauty" };
            var books = new Category { CategoryName = "Books" };
            var clothing = new Category { CategoryName = "Clothing" };
            var electronics = new Category { CategoryName = "Electronics" };
            var home = new Category { CategoryName = "Home" };
            var tools = new Category { CategoryName = "Tools" };
            var sports = new Category { CategoryName = "Sports" };
            var games = new Category { CategoryName = "Games" };
            var toys = new Category { CategoryName = "Toys" };
            var categories = new Category[]
            {
                audio,
                baby,
                beauty,
                books,
                clothing,
                electronics,
                home,
                tools,
                sports,
                games,
                toys
            };
            karrotDbContext.AddRange(categories);
            karrotDbContext.SaveChanges();
        }
    }
}