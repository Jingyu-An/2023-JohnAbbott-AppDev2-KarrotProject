using Braintree;

namespace Karrot.Services;

public interface IBraintreeService
{
    IBraintreeGateway CreateGateway();
    IBraintreeGateway GetGateway();    
}