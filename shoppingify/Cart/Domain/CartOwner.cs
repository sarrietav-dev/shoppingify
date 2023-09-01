namespace Shoppingify.Cart.Domain;

public class CartOwner
{
    private CartId? _activeCartId;
    public required CartOwnerId Id { get; init; }

    public CartId? ActiveCartId
    {
        get => _activeCartId;
        init => _activeCartId = value;
    }

    public Cart CreateCart(string name, IEnumerable<CartItem>? cartItems)
    {
        if (ActiveCartId is not null)
            throw new InvalidOperationException("Cannot create a new cart while there is an active one");

        var cart = new Cart
        {
            Id = new CartId(Guid.NewGuid()),
            CartOwnerId = Id,
            Name = name
        };

        _activeCartId = cart.Id;

        if (cartItems is not null)
            cart.UpdateList(cartItems);

        return cart;
    }
    
    public void CompleteCart()
    {
        if (ActiveCartId is null)
            throw new InvalidOperationException("Cannot complete a cart while there is no active one");

        _activeCartId = null;
    }
    
    public void CancelCart()
    {
        if (ActiveCartId is null)
            throw new InvalidOperationException("Cannot cancel a cart while there is no active one");

        _activeCartId = null;
    }
}