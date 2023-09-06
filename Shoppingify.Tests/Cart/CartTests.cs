using shoppingify.Cart.Domain;
using Product = shoppingify.Cart.Domain.Product;

namespace Shoppingify.Tests.Cart;

public class CartTests
{
    [Fact]
    public void Should_Create_Cart()
    {
        var cart = new shoppingify.Cart.Domain.Cart
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
    public void Should_Be_Equal()
    {
        var id = Guid.NewGuid();

        var cart1 = new shoppingify.Cart.Domain.Cart
        {
            Id = new CartId(id),
            CartOwnerId = new CartOwnerId("y1281"),
            Name = "My Cart"
        };

        var cart2 = new shoppingify.Cart.Domain.Cart
        {
            Id = new CartId(id),
            CartOwnerId = new CartOwnerId("y1281"),
            Name = "My Cart"
        };

        Assert.Equal(cart1, cart2);
    }

    [Fact]
    public void Should_Update_List()
    {
        var cart = new shoppingify.Cart.Domain.Cart
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
}