namespace shoppingify.Entities;

public class LineItem
{
    private Guid Id { get; } = Guid.NewGuid();
    public Product Product { get; init; } = null!;
    private int _quantity = 1;
    public int Quantity
    {
        get => _quantity;
        set
        {
            if (value < 1)
            {
                throw new ArgumentException("Quantity must be greater than 0");
            }
            _quantity = value;
        }
    }

    public bool IsChecked { get; private set; }
    
    public void IncreaseQuantity(int quantity = 1)
    {
        _quantity += quantity;
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