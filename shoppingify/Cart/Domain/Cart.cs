using System.Runtime.CompilerServices;

namespace Shoppingify.Cart.Domain;

public class Cart
{
    private CartState _state = CartState.Active;
    private ICollection<CartItem> _cartItems = new List<CartItem>();
    public required CartId Id { get; init; }
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
    public required CartOwnerId CartOwnerId { get; init; }
    public required string Name { get; init; }

    public ICollection<CartItem> CartItems
    {
        get => _cartItems;
        init => _cartItems = value ?? throw new InvalidOperationException("Cart items cannot be null");
    }

    public CartState State
    {
        get => _state;
        init =>
            _state = SetCartState(value);
    }

    private CartState SetCartState(CartState state)
    {
        return State switch
        {
            CartState.Completed => throw new InvalidOperationException("Cannot change state of a completed cart"),
            CartState.Canceled => throw new InvalidOperationException("Cannot change state of a canceled cart"),
            _ => state
        };
    }

    public void Complete()
    {
        _state = SetCartState(CartState.Completed);
    }

    public void Cancel()
    {
        _state = SetCartState(CartState.Canceled);
    }

    public void UpdateList(IEnumerable<CartItem> cartItems)
    {
        if (State != CartState.Active)
            throw new InvalidOperationException("Cannot update a non-active cart");

        _cartItems = cartItems.ToList();
    }

    public override bool Equals(object? obj)
    {
        if (obj == null || GetType() != obj.GetType())
            return false;

        var cart = (Cart)obj;
        return Id.Equals(cart.Id) && CartOwnerId.Equals(cart.CartOwnerId);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Id, CartOwnerId);
    }
}