using FSH.Framework.Core.Exceptions;

namespace FluentPos.Cart.Application.Exceptions;
internal class CartNotFoundException : NotFoundException
{
    public CartNotFoundException(object customerId) : base($"Cart for Customer '{customerId}' is not found.")
    {
    }
}
