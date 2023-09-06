namespace shoppingify.Cart.Domain;

public class CartOwner
{
    private CartId? _activeCartId;
    public required CartOwnerId Id { get; init; }

    public CartId? ActiveCartId
    {
        get => _activeCartId;
        init => _activeCartId = value;
    }

    /// <summary>
    /// Creates a new cart for the cart owner
    /// </summary>
    /// <param name="name">
    /// The name of the cart
    /// </param>
    /// <param name="cartItems">
    /// Initial list of cart items
    /// </param>
    /// <exception cref="InvalidOperationException">
    /// Thrown when the cart owner already has an active cart 
    /// </exception>
    public Cart CreateCart(string name, IEnumerable<CartItem> cartItems)
    {
        var cart = BuildCart(name);

        cart.UpdateList(cartItems);

        return cart;
    }

    /// <summary>
    /// Creates a new cart for the cart owner
    /// </summary>
    /// <param name="name">
    /// The name of the cart
    /// </param>
    /// <exception cref="InvalidOperationException">
    /// Thrown when the cart owner already has an active cart 
    /// </exception>
    public Cart CreateCart(string name)
    {
        return BuildCart(name);
    }

    private Cart BuildCart(string name)
    {
        if (ActiveCartId is not null)
            throw new InvalidOperationException("Cannot create a new cart while there is an active one");

        return new Cart
        {
            Id = new CartId(Guid.NewGuid()),
            CartOwnerId = Id,
            Name = name
        };
    }


    /// <summary>
    /// Completes the active cart.
    /// </summary>
    /// <param name="cart">
    /// Cart to be completed. Must be the owner's active cart or else an error will be thrown.
    /// </param>
    /// <exception cref="InvalidOperationException">
    /// When the Owner doesn't have an active cart, or when the given cart is already completed or canceled.
    /// </exception>
    public void CompleteCart(Cart cart)
    {
        if (ActiveCartId is null)
            throw new InvalidOperationException("Cannot complete a cart while there is no active one");

        cart.Complete();

        _activeCartId = null;
    }

    /// <summary>
    /// Cancels the active cart.
    /// </summary>
    /// <param name="cart">
    /// Cart to be canceled. Must be the owner's active cart or else an error will be thrown.
    /// </param>
    /// <exception cref="InvalidOperationException">
    /// When the Owner doesn't have an active cart, or when the given cart is already canceled or completed.
    /// </exception>
    public void CancelCart(Cart cart)
    {
        if (ActiveCartId is null)
            throw new InvalidOperationException("Cannot cancel a cart while there is no active one");
        
        cart.Cancel();

        _activeCartId = null;
    }
}