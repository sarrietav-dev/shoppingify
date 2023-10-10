namespace Shoppingify.Cart.Domain;

public record CartItem
{
    private readonly int _quantity;
    public required Product Product { get; init; }
    public required int Quantity
    {
        get => _quantity;
        init
        {
            if (value < 0)
                throw new InvalidOperationException("Quantity cannot be negative");
            
            _quantity = value;
        }
    }

    public required CartItemStatus Status { get; init; } 
}