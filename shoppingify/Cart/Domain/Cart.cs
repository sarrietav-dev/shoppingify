namespace Shoppingify.Cart.Domain;

public class Cart
{
    private CartState _state = CartState.Active;
    public required CartId Id { get; init; }
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
    public required CartOwnerId CartOwnerId { get; init; }
    public required string Name { get; init; }
    public ICollection<CartItem> CartItems { get; private set; } = new List<CartItem>();

    public CartState State
    {
        get => _state;
        private set
        {
            _state = _state switch
            {
                CartState.Completed => throw new InvalidOperationException("Cannot change state of a completed cart"),
                CartState.Canceled => throw new InvalidOperationException("Cannot change state of a canceled cart"),
                _ => value
            };
        }
    }

    public void Complete()
    {
        State = CartState.Completed;
    }

    public void Cancel()
    {
        State = CartState.Canceled;
    }

    public void UpdateList(IEnumerable<CartItem> cartItems)
    {
        if (State != CartState.Active)
            throw new InvalidOperationException("Cannot update a non-active cart");

        CartItems = cartItems.ToList();
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