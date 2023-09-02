using shoppingify.Cart.Domain;

namespace Shoppingify.Tests.Cart;

public class CartOwnerTests
{
    [Fact]
    public void Should_Create_CartOwner()
    {
        var cartOwner = new CartOwner
        {
            Id = new CartOwnerId("y1281")
        };

        Assert.Equal("y1281", cartOwner.Id.Value);
        Assert.Null(cartOwner.ActiveCartId);
    }
    
    [Fact]
    public void Should_Create_Cart()
    {
        var cartOwner = new CartOwner
        {
            Id = new CartOwnerId("y1281")
        };

        var cart = cartOwner.CreateCart("My Cart", null);

        Assert.Equal("My Cart", cart.Name);
        Assert.Equal(CartState.Active, cart.State);
        Assert.Equal(0, cart.CartItems.Count);
        Assert.Equal(cart.Id, cartOwner.ActiveCartId);
    }
    
    [Fact]
    public void Should_Complete_Cart()
    {
        var cartOwner = new CartOwner
        {
            Id = new CartOwnerId("y1281")
        };

        var cart = cartOwner.CreateCart("My Cart", null);
        cartOwner.CompleteCart();

        Assert.Null(cartOwner.ActiveCartId);
    }
    
    [Fact]
    public void Should_Cancel_Cart()
    {
        var cartOwner = new CartOwner
        {
            Id = new CartOwnerId("y1281")
        };

        var cart = cartOwner.CreateCart("My Cart", null);
        cartOwner.CancelCart();

        Assert.Null(cartOwner.ActiveCartId);
    }
}