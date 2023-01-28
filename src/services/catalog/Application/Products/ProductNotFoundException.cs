using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using FSH.Core.Exceptions;

namespace Catalog.Application.Products;

public class ProductNotFoundException : NotFoundException
{
    public ProductNotFoundException(Guid id) : base(string.Format("Queried Product Not Found, Key: {0}", id))
    {
    }
}
