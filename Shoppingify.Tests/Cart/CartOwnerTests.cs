﻿// FILEPATH: /c:/Users/ASUS/GitHub/shoppingify/Shoppingify.Tests/Cart/CartOwnerTest.cs

using Bogus;
using shoppingify.Cart.Domain;

namespace Shoppingify.Tests.Cart;

public class CartOwnerTest
{
    private readonly Faker<CartOwner> _cartOwnerFaker;
    private readonly Faker<CartItem> _cartItemFaker;

    public CartOwnerTest()
    {
        _cartOwnerFaker = new Faker<CartOwner>()
            .RuleFor(co => co.Id, f => new CartOwnerId(f.Random.String()));

        _cartItemFaker = new Faker<CartItem>()
            .RuleFor(ci => ci.Product, f => new Product(f.Random.Guid(), f.Commerce.ProductName()))
            .RuleFor(ci => ci.Quantity, f => f.Random.Int(1, 10));
    }

    [Fact]
    public void CartOwner_CreateCart_CreatesNewCart()
    {
        // Arrange
        var cartOwner = _cartOwnerFaker.Generate();
        var cartItems = _cartItemFaker.Generate(10);
        // Act
        cartOwner.CreateCart("New Cart", cartItems);

        // Assert
        Assert.NotNull(cartOwner.ActiveCart);
        Assert.Equal("New Cart", cartOwner.ActiveCart.Name);
        Assert.Equal(cartItems.Count, cartOwner.ActiveCart.CartItems.Count);
        Assert.True(cartItems.All(ci => cartOwner.ActiveCart.CartItems.Any(i => i.Product == ci.Product && i.Quantity == ci.Quantity)));
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
}