﻿namespace shoppingify.Entities;

public class ShoppingCart
{
    private readonly ICollection<LineItem> _lineItems = new List<LineItem>();
    private Guid Id { get; } = Guid.NewGuid();

    public string Name { get; private set; } = default!;

    public ICollection<LineItem> LineItems => _lineItems.ToList();

    public int CartCount => LineItems.Count;
    public int ItemCount => LineItems.Sum(i => i.Quantity);
    public int CheckedItems => LineItems.Count(i => i.IsChecked);

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
    
    public void ChangeItemQuantity(Product item, int quantity)
    {
        if (quantity < 1)
        {
            throw new ArgumentException("Quantity must be greater than 0");
        }
        
        var lineItem = _lineItems.FirstOrDefault(i => Equals(i.Product, item));

        if (lineItem != null)
        {
            lineItem.Quantity = quantity;
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

    public void CheckItem(Product item)
    {
        var lineItem = _lineItems.FirstOrDefault(i => Equals(i.Product, item));

        lineItem?.Check();
    }
}