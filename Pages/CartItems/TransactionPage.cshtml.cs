using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Karrot.Data;
using Karrot.Models;
using System.Security.Claims;
using System.ComponentModel.DataAnnotations;
using Braintree;
using Karrot.Services;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Controller;
using Decimal = System.Decimal;
using Transaction = Karrot.Pages.Contoller.Transaction;

namespace Karrot.Pages.CartItems
{
    public class TransactionPage : PageModel
    {
        private readonly KarrotDbContext context;
        private readonly ILogger<TransactionPage> logger;
        private readonly IBraintreeService braintreeService;

        public TransactionPage(ILogger<TransactionPage> logger, KarrotDbContext context,
            IBraintreeService braintreeService)
        {
            this.context = context;
            this.logger = logger;
            this.braintreeService = braintreeService;
        }

        // [BindProperty]
        // public int CartItemId { get; set; }
        public IList<CartItem> CartItems { get; set; } = default!;

        [BindProperty] 
        public InputModel Input { get; set; }
        
        [BindProperty]
        public string Nonce { get; set; }

        [BindProperty]
        public double Total { get; set; }

        public Controller Controller { get; set; }
        
        public class InputModel
        {
            [Required] public string firstName { get; set; }
            [Required] public string lastName { get; set; }
        }

        public async Task OnGetAsync()
        {
            if (context.CartItems != null)
            {
                CartItems = await context.CartItems.Include("CartItemProduct").ToListAsync();
            }

            Controller = new Transaction(braintreeService, logger, context, CartItems);

            var gateway = braintreeService.GetGateway();
            var clientToken = gateway.ClientToken.Generate();
            //Genarate a token
            Controller.ViewBag.ClientToken = clientToken;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var userName = User.Identity.Name;
            var user = context.Users.Where(u => u.UserName == userName).FirstOrDefault();
            
            CartItems = await context.CartItems.Include("CartItemProduct").ToListAsync();
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
            var address = context.Address.FirstOrDefault(x => x.User.Id == userId);
            
            var order = new Order();
            order.OrderItems = new();
            order.Address = address.AddressLine1;
            order.City = address.City;
            order.State = address.State;
            order.Country = address.Country;
            order.PostalCode = address.PostalCode;
            order.Email = user.Email;
            order.Phone = user.PhoneNumber;
            order.FirstName = Input.firstName;
            order.LastName = Input.lastName;
            order.Total= Convert.ToDecimal(Total);
            order.PaymentTransactionId = Nonce;
            
            foreach (var item in CartItems)
            {
                order.OrderItems.Add(new OrderItem
                {
                    Product = item.CartItemProduct,
                    OrderQuantity = item.CartQuantity,
                });
            }
            
            Transaction transaction = new Transaction(braintreeService, logger, context, CartItems); 
            
            return transaction.Create(order).Result;
        }
    }
}