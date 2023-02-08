using Braintree;
using Karrot.Services;
using Microsoft.AspNetCore.Mvc;

namespace Karrot.Pages.Contoller;

public class Transaction : Controller
{
    private readonly IBraintreeService _braintreeService;

    public Transaction(IBraintreeService braintreeService)
    {
        _braintreeService = braintreeService;
    }

    public IActionResult Index()
    {
        var gateway = _braintreeService.GetGateway();
        var clientToken = gateway.ClientToken.Generate();
        //Genarate a token
        ViewBag.ClientToken = clientToken;


        return Index();
    }

    [HttpPost]
    public IActionResult Create() //(BookPurchaseVM model)
    {
        var gateway = _braintreeService.GetGateway();
        var request = new TransactionRequest
        {
            Amount = Convert.ToDecimal("250"),
            // PaymentMethodNonce = model.Nonce,
            // Options = new TransactionOptionsRequest
            // {
            //     SubmitForSettlement = true
            // }
        };

        Result<Braintree.Transaction> result = gateway.Transaction.Sale(request);

        return View("Index");
        
        // if (result.IsSuccess())
        // {
        //     return View("Success");
        // }
        // else
        // {
        //     return View("Failure");
        // }
    }

}