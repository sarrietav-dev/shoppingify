namespace Shoppingify.Cart.Domain;

public class Cart
{
    private CartState _state = CartState.Active;
    public required CartId Id { get; init; }
    public required CartOwnerId CartOwnerId { get; init; }
    public required string Name { get; init; }

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
}