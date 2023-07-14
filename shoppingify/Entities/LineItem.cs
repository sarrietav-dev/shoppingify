namespace shoppingify.Entities;

public class LineItem
{
    public Guid Id { get; private set; }
    public Product Product { get; init; } = null!;
    public int Quantity { get; private set; } = 1;
    
    public void IncreaseQuantity()
    {
        Quantity++;
    }
}