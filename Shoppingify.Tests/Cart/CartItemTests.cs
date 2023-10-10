using Bogus;
using Shoppingify.Cart.Domain;

namespace Shoppingify.Tests.Cart;

public class CartItemTest
{
    private readonly Faker<CartItem> _cartItemFaker;

    public CartItemTest()
    {
        _cartItemFaker = new Faker<CartItem>()
            .RuleFor(ci => ci.Product, f => new Product(f.Random.Guid(), f.Commerce.ProductName()))
            .RuleFor(ci => ci.Quantity, f => f.Random.Int(1, 10));
    }

    [Fact]
    public void CartItem_CreateCartItem_ReturnsCorrectValues()
    {
        // Arrange
        var product = new Product(Guid.NewGuid(), "Product Name");
        var quantity = 5;

        // Act
        var cartItem = new CartItem { Product = product, Quantity = quantity, Status = CartItemStatus.Unchecked };

        // Assert
        Assert.Equal(product, cartItem.Product);
        Assert.Equal(quantity, cartItem.Quantity);
    }

    [Fact]
    public void CartItem_CreateCartItem_ThrowsExceptionWhenQuantityIsNegative()
    {
        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => _cartItemFaker.RuleFor(ci => ci.Quantity, -1).Generate());
    }
}