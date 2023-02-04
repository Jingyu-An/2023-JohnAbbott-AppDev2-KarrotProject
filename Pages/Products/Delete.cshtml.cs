using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs.Specialized;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Karrot.Data;
using Karrot.Models;
using Microsoft.AspNetCore.Authorization;

namespace Karrot.Pages.Products
{
    [Authorize]
    public class DeleteModel : PageModel
    {
        private readonly KarrotDbContext context;
        private ILogger<DeleteModel> logger;
        private readonly string storageConnectionString;
        private readonly string storageContainerName;

        public DeleteModel(KarrotDbContext context, ILogger<DeleteModel> logger,
            IConfiguration configuration)
        {
            this.context = context;
            this.logger = logger;
            storageConnectionString = configuration.GetValue<string>("BlobConnectionString");
            storageContainerName = configuration.GetValue<string>("BlobContainerName");
        }

        [BindProperty] public Product Product { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || context.Products == null)
            {
                return NotFound();
            }

            var product = await context.Products.FirstOrDefaultAsync(m => m.Id == id);

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

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || context.Products == null)
            {
                return NotFound();
            }

            var product = await context.Products.FindAsync(id);
            var container = new BlobContainerClient(storageConnectionString, storageContainerName);
            var imageUrl = product.Image;
            string filename = "";

            if (imageUrl != null)
            {
                Match match = Regex.Match(imageUrl, @"([^/]+\.[^/]+)$");
                filename = match.Groups[1].Value;

                var blcokBlobClient = container.GetBlockBlobClient(filename);
                blcokBlobClient.DeleteIfExists(DeleteSnapshotsOption.IncludeSnapshots);
                logger.LogInformation("Old image deleted");
            }

            if (product != null)
            {
                Product = product;
                context.Products.Remove(Product);
                await context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}