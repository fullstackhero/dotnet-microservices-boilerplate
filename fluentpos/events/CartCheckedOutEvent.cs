using FSH.Framework.Core.Events;

namespace FluentPos.Shared.Events;
public class CartCheckedOutEvent : IntegrationEvent
{
    public Guid CustomerId { get; }
    public string CreditCardNumber { get; }

    public CartCheckedOutEvent(Guid customerId, string creditCardNumber)
    {
        CustomerId = customerId;
        CreditCardNumber = creditCardNumber;
    }
}
