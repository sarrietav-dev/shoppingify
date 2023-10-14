// FILEPATH: /c:/Users/ASUS/GitHub/shoppingify/Shoppingify.Tests/Cart/CartOwnerTest.cs

using Bogus;
using Shoppingify.Cart.Domain;

namespace Shoppingify.Tests.Cart;

public class CartOwnerTest
{
    private readonly Faker<CartItem> _cartItemFaker;
    private readonly Faker<CartOwner> _cartOwnerFaker;

    public CartOwnerTest()
    {
        _cartOwnerFaker = new Faker<CartOwner>()
            .RuleFor(co => co.Id, f => new CartOwnerId(f.Random.String()));

        _cartItemFaker = new Faker<CartItem>()
            .RuleFor(ci => ci.Product, f => new Product(f.Random.Guid(), f.Commerce.ProductName()))
            .RuleFor(ci => ci.Quantity, f => f.Random.Int(1, 10));
    }

    [Fact]
    public void CartOwner_WithoutActiveCart_CreatesSuccessfully()
    {
        // Arrange
        var cartOwner = _cartOwnerFaker.Generate();

        // Assert
        Assert.Null(cartOwner.ActiveCart);
    }

    [Fact]
    public void CartOwner_CreateCart_CreatesNewCart()
    {
        // Arrange
        var cartOwner = _cartOwnerFaker.Generate();
        var cartItems = _cartItemFaker.Generate(10);
        // Act
        var cart = cartOwner.CreateCart("New Cart", cartItems);

        // Assert
        Assert.Equal(cart.Id, cartOwner.ActiveCart);
        Assert.Equal("New Cart", cart.Name);
        Assert.Equal(cartItems.Count, cart.CartItems.Count);
        Assert.True(cartItems.All(ci =>
            cart.CartItems.Any(i => i.Product == ci.Product && i.Quantity == ci.Quantity)));
    }

    [Fact]
    public void CartOwner_CreateCart_ThrowsExceptionWhenActiveCartExists()
    {
        // Arrange
        var cartOwner = _cartOwnerFaker.Generate();
        var cartItems1 = _cartItemFaker.Generate(2);
        var cartItems2 = _cartItemFaker.Generate(2);
        cartOwner.CreateCart("Cart 1", cartItems1);

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => cartOwner.CreateCart("Cart 2", cartItems2));
    }

    [Fact]
    public void CompleteCart_WithValidData_Successfully()
    {
        // Arrange
        var cartOwner = _cartOwnerFaker.Generate();
        var cartItems = _cartItemFaker.Generate(10);
        cartOwner.CreateCart("New Cart", cartItems);

        // Act
        cartOwner.CompleteCart();

        // Assert
        Assert.Null(cartOwner.ActiveCart);
    }

    [Fact]
    public void CompleteCart_CartOwnerWithoutActiveCart_ThrowsException()
    {
        // Arrange
        var cartOwner = _cartOwnerFaker.Generate();

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => cartOwner.CompleteCart());
    }

    [Fact]
    public void CancelCart_WithValidData_Successfully()
    {
        // Arrange
        var cartOwner = _cartOwnerFaker.Generate();
        var cartItems = _cartItemFaker.Generate(10);
        cartOwner.CreateCart("New Cart", cartItems);

        // Act
        cartOwner.CancelCart();

        // Assert
        Assert.Null(cartOwner.ActiveCart);
    }

    [Fact]
    public void CancelCart_CartOwnerWithoutActiveCart_ThrowsException()
    {
        // Arrange
        var cartOwner = _cartOwnerFaker.Generate();

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => cartOwner.CancelCart());
    }
}