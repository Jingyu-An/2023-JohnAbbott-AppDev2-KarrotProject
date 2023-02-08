using System.Collections.Generic;
using System.Threading.Tasks;
using Braintree;
using Karrot.Data;
using Karrot.Models;
using Karrot.Pages.CartItems;
using Karrot.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Karrot.Pages.Contoller;

public class Transaction : Controller
{
    private readonly IBraintreeService _braintreeService;
    private readonly ILogger<TransactionPage> _logger;
    private readonly KarrotDbContext context;
    private IList<CartItem> CartItems { get; set; }

    public Transaction(IBraintreeService braintreeService, ILogger<TransactionPage> logger,
        KarrotDbContext context, IList<CartItem> cartItems)
    {
        _braintreeService = braintreeService;
        _logger = logger;
        this.context = context;
        CartItems = cartItems;
    }

    [HttpPost]
    public async Task<IActionResult> Create(Order order)
    {
        var gateway = _braintreeService.GetGateway();
        var request = new TransactionRequest
        {
            Amount = order.Total,
            PaymentMethodNonce = order.PaymentTransactionId,
            Options = new TransactionOptionsRequest
            {
                SubmitForSettlement = true
            }
        };

        Result<Braintree.Transaction> result = gateway.Transaction.Sale(request);
        
        if (result.IsSuccess())
        {
            context.Orders.Add(order);
            context.CartItems.RemoveRange(CartItems);
            await context.SaveChangesAsync();
            
            _logger.LogInformation("Transaction success");
            
            return RedirectToPage("OrderSummary", new { Id = order.OrderId });
        }

        _logger.LogError("Transaction fail");
        return Redirect("TransactionPage");
    }
}