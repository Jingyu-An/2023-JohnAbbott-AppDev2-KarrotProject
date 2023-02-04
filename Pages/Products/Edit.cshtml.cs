using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs.Specialized;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Karrot.Data;
using Karrot.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Build.Framework;

namespace Karrot.Pages.Products
{
    [Authorize]
    public class EditModel : PageModel
    {
        private readonly KarrotDbContext context;
        private ILogger<EditModel> logger;
        private readonly string storageConnectionString;
        private readonly string storageContainerName;

        public EditModel(KarrotDbContext context, ILogger<EditModel> logger,
            IConfiguration configuration)
        {
            this.context = context;
            this.logger = logger;
            storageConnectionString = configuration.GetValue<string>("BlobConnectionString");
            storageContainerName = configuration.GetValue<string>("BlobContainerName");
        }

        [BindProperty(SupportsGet = true)] public int Id { get; set; }

        [BindProperty] public Product Product { get; set; }

        [BindProperty] public int Category { get; set; }

        [BindProperty] public int Address { get; set; }

        [BindProperty] 
        public IFormFile? Image { get; set; }

        public List<Category> Categories { get; set; }
        public int CategoryId { get; set; }
        public List<Address>? Addresses { get; set; }
        public int AddressId { get; set; }

        public string ImageUrl { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || context.Products == null)
            {
                return NotFound();
            }

            var product = await context.Products.Include("Owner").Include("Category").Include("Address")
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            Product = product;
            CategoryId = Product.Category.CategoryId;
            AddressId = Product.Address.AddressId;
            ImageUrl = Product.Image;

            if (context.Categories != null)
            {
                Categories = await context.Categories.ToListAsync();
            }

            if (context.Address != null)
            {
                Addresses = await context.Address.Where(u => u.User.UserName == Product.Owner.UserName).ToListAsync();
            }

            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            var product = await context.Products.Include("Owner").Include("Category").Include("Address")
                .FirstOrDefaultAsync(m => m.Id == Id);
            Categories = await context.Categories.ToListAsync();
            Addresses = await context.Address.Where(u => u.User.UserName == product.Owner.UserName).ToListAsync();
            ImageUrl = product.Image;

            ModelState.Remove("Product.Owner");
            ModelState.Remove("Product.Category");
            ModelState.Remove("Product.Address");
            ModelState.Remove("Product.Image");
            ModelState.Remove("Image");

            if (!ModelState.IsValid)
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
                var newFileName = String
                    .Join("_", Image.FileName.Split(invalids, StringSplitOptions.RemoveEmptyEntries))
                    .TrimEnd('.');

                var uploadFileName = User.Identity.Name + "_" + DateTime.Now.ToString("yyyy-MM-dd-hh-mm-ss-fff") + "_" +
                                     newFileName;

                var container = new BlobContainerClient(storageConnectionString, storageContainerName);
                try
                {
                    string filename = "";

                    if (ImageUrl != null)
                    {
                        Match match = Regex.Match(ImageUrl, @"([^/]+\.[^/]+)$");
                        filename = match.Groups[1].Value;
                        
                        var blcokBlobClient = container.GetBlockBlobClient(filename);
                        blcokBlobClient.DeleteIfExists(DeleteSnapshotsOption.IncludeSnapshots);
                        logger.LogInformation("Old image deleted");
                    }

                    var blob = container.GetBlobClient(uploadFileName);
                    await using (Stream? data = Image.OpenReadStream())
                    {
                        await blob.UploadAsync(data);
                    }

                    ImageUrl = blob.Uri.ToString();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }

            var category = context.Categories.Where(c => c.CategoryId == Category).FirstOrDefault();
            var address = context.Address.Where(a => a.AddressId == Address).FirstOrDefault();

            product.ProductName = Product.ProductName;
            product.ProductDescription = Product.ProductDescription;
            product.ProductPrice = Product.ProductPrice;
            product.Image = ImageUrl;
            product.Address = address;
            product.Category = category;
            product.CreateAt = DateTime.Now;

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(Product.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool ProductExists(int id)
        {
            return (context.Products?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}