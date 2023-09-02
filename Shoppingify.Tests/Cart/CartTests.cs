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
    public void Should_Complete()
    {
        var cart = new shoppingify.Cart.Domain.Cart
        {
            Id = new CartId(Guid.NewGuid()),
            CartOwnerId = new CartOwnerId("y1281"),
            Name = "My Cart"
        };

        cart.Complete();

        Assert.Equal(CartState.Completed, cart.State);
    }

    [Fact]
    public void Should_Cancel()
    {
        var cart = new shoppingify.Cart.Domain.Cart
        {
            Id = new CartId(Guid.NewGuid()),
            CartOwnerId = new CartOwnerId("y1281"),
            Name = "My Cart"
        };

        cart.Cancel();

        Assert.Equal(CartState.Canceled, cart.State);
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

    [Fact]
    public void Should_Not_Complete_Twice()
    {
        var cart = new shoppingify.Cart.Domain.Cart
        {
            Id = new CartId(Guid.NewGuid()),
            CartOwnerId = new CartOwnerId("y1281"),
            Name = "My Cart"
        };

        var exception = Assert.Throws<InvalidOperationException>(Act);

        Assert.Equal("Cannot change state of a completed cart", exception.Message);
        return;

        void Act()
        {
            cart.Complete();
            cart.Complete();
        }
    }
    
    [Fact]
    public void Should_Not_Cancel_Twice()
    {
        var cart = new shoppingify.Cart.Domain.Cart
        {
            Id = new CartId(Guid.NewGuid()),
            CartOwnerId = new CartOwnerId("y1281"),
            Name = "My Cart"
        };

        var exception = Assert.Throws<InvalidOperationException>(Act);

        Assert.Equal("Cannot change state of a canceled cart", exception.Message);
        return;

        void Act()
        {
            cart.Cancel();
            cart.Cancel();
        }
    }
}