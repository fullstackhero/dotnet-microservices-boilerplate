using Catalog.Domain.Entities;
using FluentAssertions;

namespace Catalog.UnitTests.Domain;

public class CreateProductTests
{
    [Fact]
    public void Product_Create()
    {
        // Act
        var product = Product.Create("Samsung S22", 1, 20);

        // Assert
        product.Name.Should().Be("Samsung S22");
        product.Quantity.Should().Be(1);
        product.Price.Should().Be(20);
    }

    [Fact]
    public void Product_Update()
    {
        //Arrange
        var product = Product.Create("Samsung S22", 1, 20);

        // Act
        product.Update("Samsung S23", 2, 40);

        // Assert
        product.Name.Should().Be("Samsung S23");
        product.Quantity.Should().Be(2);
        product.Price.Should().Be(40);
    }
}
