using FluentAssertions;
using FluentPOS.Lite.Catalog.Domain.Entities;

namespace FluentPOS.Lite.Catalog.UnitTests.Domain;

public class ProductTests
{
    [Fact]
    public void Product_Create()
    {
        // Act
        var product = Product.Create("Samsung S22", "test", "CC1", 10, 12, 1);

        // Assert
        product.Name.Should().Be("Samsung S22");
        product.Quantity.Should().Be(1);
        product.Price.Should().Be(12);
    }

    [Fact]
    public void Product_Update()
    {
        //Arrange
        var product = Product.Create("Samsung S22", "test", "CC1", 10, 12, 1);

        // Act
        product.Update("Samsung S23", 40);

        // Assert
        product.Name.Should().Be("Samsung S23");
        product.Price.Should().Be(40);
    }
}
