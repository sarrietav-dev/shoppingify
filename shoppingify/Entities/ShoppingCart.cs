namespace shoppingify.Entities;

public class ShoppingCart
{
    private ICollection<LineItem> _lineItems = new List<LineItem>();
    public Guid Id { get; }

    public string Name { get; private set; } = default!;

    public ICollection<LineItem> LineItems
    {
        get => _lineItems;
        private set => _lineItems = value;
    }
    
    public int CartCount => LineItems.Count;

    public void AddItem(Product item)
    {
        var lineItem = LineItems.FirstOrDefault(i => i.Product.Id == item.Id);
        if (lineItem != null)
        {
            lineItem.IncreaseQuantity();
        }
        else
        {
            LineItems.Add(new LineItem
            {
                Product = item,
            });
        }
    }
}