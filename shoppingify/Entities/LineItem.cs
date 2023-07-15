namespace shoppingify.Entities;

public class LineItem
{
    private Guid Id { get; } = Guid.NewGuid();
    public Product Product { get; init; } = null!;
    public int Quantity { get; private set; } = 1;
    public bool IsChecked { get; private set; } = false;
    
    public void IncreaseQuantity(int quantity = 1)
    {
        Quantity += quantity;
    }
    
    public void DecreaseQuantity(int quantity = 1)
    {
        Quantity -= quantity;
    }

    public override bool Equals(object? obj)
    {
        return obj is LineItem item &&
               EqualityComparer<Guid>.Default.Equals(Id, item.Id);
    }

    public void Check()
    {
        IsChecked = true;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Id);
    }
}