using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection.Metadata;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Karrot.Data;
using Karrot.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Karrot.Pages.Products
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly KarrotDbContext context;
        private ILogger<CreateModel> logger;
        private readonly string storageConnectionString;
        private readonly string storageContainerName;

        public CreateModel(KarrotDbContext context, ILogger<CreateModel> logger,
            IConfiguration configuration)
        {
            this.context = context;
            this.logger = logger;
            storageConnectionString = configuration.GetValue<string>("BlobConnectionString");
            storageContainerName = configuration.GetValue<string>("BlobContainerName");
        }

        [BindProperty, Required] public string Name { get; set; }

        [BindProperty, Required] public string Description { get; set; }

        [BindProperty, Required] public double Price { get; set; }

        [BindProperty, Required] public IFormFile Image { get; set; }
        
        [BindProperty, Required] public int Category { get; set; }
        [BindProperty, Required] public int Address { get; set; }
        public List<Category>? Categories { get; set; }
        public List<Address>? Addresses { get; set; }

        public async Task OnGetAsync()
        {
            if (context.Categories != null)
            {
                Categories = await context.Categories.ToListAsync();
            }

            if (context.Address != null)
            {
                Addresses = await context.Address.Where(u => u.User.UserName == User.Identity.Name).ToListAsync();
            }
        }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            Categories = await context.Categories.ToListAsync();
            string url = "";

            if (!ModelState.IsValid || context.Products == null || context.Categories == null)
            {
                return Page();
            }

            if (Image != null)
            {
                string fileExtension = Path.GetExtension(Image.FileName).ToLower();
                string[] allowedExtensions = { ".jpg", ".jpeg", ".gif", ".png" };

                if (!allowedExtensions.Contains(fileExtension))
                {
                    ModelState.AddModelError(string.Empty, "Only Image files" +
                                                           "(jpg, jpeg, gif, png) are allowed");
                    return Page();
                }

                var invalids = Path.GetInvalidPathChars();
                var newFileName = String.Join("_", Image.FileName.Split(invalids, StringSplitOptions.RemoveEmptyEntries))
                    .TrimEnd('.');
                
                var uploadFileName = User.Identity.Name + "_" + DateTime.Now.ToString("yyyy-MM-dd-hh-mm-ss-fff") + "_" + newFileName;
                
                var container = new BlobContainerClient(storageConnectionString, storageContainerName);
                try
                {
                    var blob = container.GetBlobClient(uploadFileName);

                    await using (Stream? data = Image.OpenReadStream())
                    {
                        await blob.UploadAsync(data);
                    }
                    Match match = Regex.Match(url, @"([^/]+\.[^/]+)$");
                    string filename = match.Groups[1].Value;

                    container.DeleteBlob(filename);
                    url = blob.Uri.ToString();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }

            var userName = User.Identity.Name;
            var user = context.Users.Where(u => u.UserName == userName).FirstOrDefault();
            var category = context.Categories.Where(c => c.CategoryId == Category).FirstOrDefault();
            var address = context.Address.Where(a => a.AddressId == Address).FirstOrDefault();
            
            logger.LogInformation(
                $"{Name}, {Description}, {Price}, {Image.FileName}, {category.CategoryName}, {user.UserName}, {url}");

            var newProduct = new Product
            {
                Owner = user, ProductName = Name, ProductDescription = Description, Image = url,
                ProductPrice = Price, Category = category, CreateAt = DateTime.Now, Address = address
            };

            context.Products.Add(newProduct);
            await context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}