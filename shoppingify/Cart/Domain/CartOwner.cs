namespace Shoppingify.Cart.Domain;

public class CartOwner
{
    private Cart? _activeCart;
    public required CartOwnerId Id { get; init; }

    public Cart? ActiveCart
    {
        get => _activeCart;
        init => _activeCart = value;
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
    public void CreateCart(string name, IEnumerable<CartItem> cartItems)
    {
        var cart = BuildCart(name);

        cart.UpdateList(cartItems);

        _activeCart = cart;
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
    public void CreateCart(string name)
    {
        _activeCart = BuildCart(name);
    }

    private Cart BuildCart(string name)
    {
        if (_activeCart is not null)
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
    /// <exception cref="InvalidOperationException">
    /// When the Owner doesn't have an active cart, or when the given cart is already completed or canceled.
    /// </exception>
    public Cart CompleteCart()
    {
        if (_activeCart is null)
            throw new InvalidOperationException("Cannot complete a cart while there is no active one");

        _activeCart.Complete();

        var cart = _activeCart;

        _activeCart = null;

        return cart;
    }

    /// <summary>
    /// Cancels the active cart.
    /// </summary>
    /// <exception cref="InvalidOperationException">
    /// When the Owner doesn't have an active cart, or when the given cart is already canceled or completed.
    /// </exception>
    public Cart CancelCart()
    {
        if (_activeCart is null)
            throw new InvalidOperationException("Cannot cancel a cart while there is no active one");

        _activeCart.Cancel();

        var cart = _activeCart;

        _activeCart = null;

        return cart;
    }
}