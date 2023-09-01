using Shoppingify.Products.Domain;

namespace Shoppingify.Cart.Domain;

public class CartItem
{
    public required CartItemId Id { get; init; }
    public required CartId CartId { get; init; }
    public required Product Product { get; init; }
    public required int Quantity { get; init; }
    public CartItemStatus Status { get; private set; } = CartItemStatus.Unchecked;
    
    public void Check()
    {
        Status = CartItemStatus.Checked;
    }
    
    public void Uncheck()
    {
        Status = CartItemStatus.Unchecked;
    }
}