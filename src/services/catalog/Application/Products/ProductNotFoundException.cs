using System.Net;
using FSH.Core.Exceptions;

namespace Catalog.Application.Products;

public class ProductNotFoundException : CustomException
{
    public ProductNotFoundException(Guid id) : base(string.Format("Queried Product Not Found, Key: {0}", id), HttpStatusCode.NotFound)
    {
    }
}
