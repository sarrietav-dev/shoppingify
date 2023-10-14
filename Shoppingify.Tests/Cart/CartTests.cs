using Shoppingify.Cart.Domain;

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
                Product = new ProductId(Guid.NewGuid()),
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
                Product = new ProductId(Guid.NewGuid()),
                Status = CartItemStatus.Checked
            }
        };

        cart.Complete();

        Assert.Throws<InvalidOperationException>(() => cart.UpdateList(cartItems));
    }

    [Fact]
    public void Complete_WithActiveState_ThrowsException()
    {
        var cart = new Shoppingify.Cart.Domain.Cart
        {
            Id = new CartId(Guid.NewGuid()),
            CartOwnerId = new CartOwnerId("y1281"),
            Name = "My Cart"
        };

        cart.Complete();

        Assert.Throws<InvalidOperationException>(() => cart.Complete());
    }

    [Fact]
    public void Cancel_WithActiveState_ThrowsException()
    {
        var cart = new Shoppingify.Cart.Domain.Cart
        {
            Id = new CartId(Guid.NewGuid()),
            CartOwnerId = new CartOwnerId("y1281"),
            Name = "My Cart"
        };

        cart.Cancel();

        Assert.Throws<InvalidOperationException>(() => cart.Cancel());
    }

    [Fact]
    public void Equals_WithDifferentId_Should_Return_False()
    {
        var cart1 = new Shoppingify.Cart.Domain.Cart
        {
            Id = new CartId(Guid.NewGuid()),
            CartOwnerId = new CartOwnerId("y1281"),
            Name = "My Cart"
        };

        var cart2 = new Shoppingify.Cart.Domain.Cart
        {
            Id = new CartId(Guid.NewGuid()),
            CartOwnerId = new CartOwnerId("y1281"),
            Name = "My Cart"
        };

        Assert.NotEqual(cart1, cart2);
    }

    [Fact]
    public void Equals_WithSameId_And_DifferentOwnerId_Should_Return_False()
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
            CartOwnerId = new CartOwnerId("y1282"),
            Name = "My Cart"
        };

        Assert.NotEqual(cart1, cart2);
    }

    [Fact]
    public void Equals_WithSameId_And_OwnerId_Should_Return_True()
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
    public void Equals_NullObject_Should_Return_False()
    {
        var cart1 = new Shoppingify.Cart.Domain.Cart
        {
            Id = new CartId(Guid.NewGuid()),
            CartOwnerId = new CartOwnerId("y1281"),
            Name = "My Cart"
        };

        Assert.False(cart1.Equals(null));
    }

    [Fact]
    public void Equals_DifferentType_Should_Return_False()
    {
        var cart1 = new Shoppingify.Cart.Domain.Cart
        {
            Id = new CartId(Guid.NewGuid()),
            CartOwnerId = new CartOwnerId("y1281"),
            Name = "My Cart"
        };

        Assert.False(cart1.Equals(new object()));
    }

    [Fact]
    public void GetHashCode_WithSameId_And_OwnerId_Should_Return_Same_HashCode()
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

        Assert.Equal(cart1.GetHashCode(), cart2.GetHashCode());
    }
}