using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Catalog.Domain.Entities;
using FluentAssertions;

namespace Catalog.UnitTests.Domain;

public class CreateProductTests
{
    [Fact]
    public void test_product_creation()
    {
        var product = Product.Create("Samsung S22", 1, 20);
        product.Name.Should().Be("Samsung S22");
        product.Quantity.Should().Be(1);
        product.Price.Should().Be(20);
    }
}
