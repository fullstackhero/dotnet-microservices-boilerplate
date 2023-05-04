using FSH.Framework.Core.Exceptions;

namespace FluentPos.Catalog.Core.Products.Exceptions;
internal class ProductNotFoundException : NotFoundException
{
    public ProductNotFoundException(object productId) : base($"Product with ID '{productId}' is not found.")
    {
    }
}
