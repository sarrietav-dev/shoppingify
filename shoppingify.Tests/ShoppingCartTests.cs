using shoppingify.Entities;

namespace shoppingify.Tests;

public class ShoppingCartTests
{
    [Fact]
    public void AddItemToCart()
    {
        // Arrange
        var cart = new ShoppingCart();
        var item = new Product
        {
            Name = "Test",
            Note = "",
            Category = "null",
            Image = "null"
        };

        // Act
        cart.AddItem(item);

        // Assert
        Assert.Equal(1, cart.CartCount);
    }
    
    [Fact]
    public void AddItemToCartTwice()
    {
        // Arrange
        var cart = new ShoppingCart();
        var item = new Product
        {
            Name = "Test",
            Note = "",
            Category = "null",
            Image = "null"
        };

        // Act
        cart.AddItem(item);
        cart.AddItem(item);

        // Assert
        Assert.Equal(1, cart.CartCount);
        Assert.Equal(2, cart.ItemCount);
    }
    
    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(100)]
    public void IncreaseItemCount(int increaseValue)
    {
        // Arrange
        var cart = new ShoppingCart();
        var item = new Product
        {
            Name = "Test",
            Note = "",
            Category = "null",
            Image = "null"
        };

        // Act
        cart.AddItem(item);
        cart.IncreaseQuantity(item, increaseValue);

        // Assert
        Assert.Equal(1, cart.CartCount);
        Assert.Equal(increaseValue + 1, cart.ItemCount);
    }

    [Fact]
    public void RemoveItemFromCart()
    {
        // Arrange
        var cart = new ShoppingCart();
        var item = new Product
        {
            Name = "Test",
            Note = "",
            Category = "null",
            Image = "null"
        };

        var item2 = new Product
        {
            Name = "Test2",
            Note = "",
            Category = "null",
            Image = "null"
        };

        // Act
        cart.AddItem(item);
        cart.AddItem(item2);

        cart.DecreaseQuantity(item);

        // Assert
        Assert.Equal(1, cart.CartCount);
        Assert.Equal(1, cart.ItemCount);
    }
    
    [Fact]
    public void RemoveItemFromList()
    {
        // Arrange
        var cart = new ShoppingCart();
        var item = new Product
        {
            Name = "Test",
            Note = "",
            Category = "null",
            Image = "null"
        };

        //Act
        cart.AddItem(item);
        cart.RemoveItem(item);

        // Assert
        Assert.Equal(0, cart.CartCount);
        Assert.Equal(0, cart.ItemCount);
    }
}