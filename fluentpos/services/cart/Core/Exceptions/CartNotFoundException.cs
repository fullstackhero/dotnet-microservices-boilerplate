using FSH.Framework.Core.Exceptions;

namespace FluentPos.Cart.Core.Exceptions;
internal class CartNotFoundException : NotFoundException
{
    public CartNotFoundException(object customerId) : base($"Cart for Customer '{customerId}' is not found.")
    {
    }
}
