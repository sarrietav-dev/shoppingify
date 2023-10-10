using Shoppingify.Cart.Domain;
using Product = Shoppingify.Cart.Domain.Product;

namespace Shoppingify.Tests.Cart;

public class CartTests
{
    [Fact]
    public void Should_Create_Cart()
    {
        var cart = new Shoppingify.Cart.Domain.Cart
        {
            Id = new CartId(Guid.NewGuid()),
            CartOwnerId = new CartOwnerId("y1281"),
            Name = "My Cart"
        };

        Assert.Equal("My Cart", cart.Name);
        Assert.Equal(CartState.Active, cart.State);
        Assert.Equal(0, cart.CartItems.Count);
    }

    [Fact]
    public void Carts_With_Same_Id_And_OwnerId_Should_Be_Equal()
    {
        var id = Guid.NewGuid();

        var cart1 = new Shoppingify.Cart.Domain.Cart
        {
            Id = new CartId(id),
            CartOwnerId = new CartOwnerId("y1281"),
            Name = "My Cart"
        };

        var cart2 = new Shoppingify.Cart.Domain.Cart
        {
            Id = new CartId(id),
            CartOwnerId = new CartOwnerId("y1281"),
            Name = "My Cart"
        };

        Assert.Equal(cart1, cart2);
    }

    [Fact]
    public void UpdateList_SingleItem_Successfully()
    {
        var cart = new Shoppingify.Cart.Domain.Cart
        {
            Id = new CartId(Guid.NewGuid()),
            CartOwnerId = new CartOwnerId("y1281"),
            Name = "My Cart"
        };

        var cartItems = new List<CartItem>
        {
            new()
            {
                Quantity = 1,
                Product = new Product(Guid.NewGuid(), "Hi"),
                Status = CartItemStatus.Checked
            }
        };

        cart.UpdateList(cartItems);

        Assert.Equal(cartItems, cart.CartItems);
    }

    [Fact]
    public void UpdateList_WithNotActiveCart_ThrowsException()
    {
        var cart = new Shoppingify.Cart.Domain.Cart
        {
            Id = new CartId(Guid.NewGuid()),
            CartOwnerId = new CartOwnerId("y1281"),
            Name = "My Cart"
        };

        var cartItems = new List<CartItem>
        {
            new()
            {
                Quantity = 1,
                Product = new Product(Guid.NewGuid(), "Hi"),
                Status = CartItemStatus.Checked
            }
        };

        cart.Complete();

        Assert.Throws<InvalidOperationException>(() => cart.UpdateList(cartItems));
    }
}