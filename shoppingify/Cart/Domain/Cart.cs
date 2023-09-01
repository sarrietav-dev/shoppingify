namespace Shoppingify.Cart.Domain;

public class Cart
{
    public required CartId Id { get; init; }
    public required CartOwnerId CartOwnerId { get; init; }
    public required string Name { get; init; }
    public CartState State { get; private set; } = CartState.Active;

    public void Complete()
    {
        State = CartState.Completed;
    }
}