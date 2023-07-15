namespace shoppingify.Entities;

public class ShoppingCart
{
    private ICollection<LineItem> _lineItems = new List<LineItem>();
    private Guid Id { get; } = Guid.NewGuid();

    public string Name { get; private set; } = default!;

    public ICollection<LineItem> LineItems
    {
        get => _lineItems.ToList();
        private set => _lineItems = value;
    }
    
    public int CartCount => LineItems.Count;
    public int ItemCount => LineItems.Sum(i => i.Quantity);

    public void AddItem(Product item)
    {
        var lineItem = LineItems.FirstOrDefault(i => Equals(i.Product, item));
        
        if (lineItem != null)
        {
            lineItem.IncreaseQuantity();
        }
        else
        {
            _lineItems.Add(new LineItem
            {
                Product = item,
            });
        }
    }

    public void IncreaseQuantity(Product item, int quantity = 1)
    {
        var lineItem = _lineItems.FirstOrDefault(i => Equals(i.Product, item));

        lineItem?.IncreaseQuantity(quantity);
    }
    
    public void DecreaseQuantity(Product item, int quantity = 1)
    {
        var lineItem = _lineItems.FirstOrDefault(i => Equals(i.Product, item));

        lineItem?.DecreaseQuantity(quantity);
        
        if (lineItem?.Quantity == 0)
        {
            RemoveItem(item);
        }
    }

    private bool Equals(ShoppingCart other)
    {
        return Id.Equals(other.Id);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((ShoppingCart)obj);
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

    public void RemoveItem(Product item)
    {
        var lineItem = _lineItems.FirstOrDefault(i => Equals(i.Product, item));

        if (lineItem != null)
        {
            _lineItems.Remove(lineItem);
        }
    }
}