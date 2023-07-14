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
    
    
}