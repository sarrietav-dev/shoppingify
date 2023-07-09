namespace shoppingify.Entities;

class ShoppingCart
{
    public Guid Id { get; }

    public string Name { get; private set; } = default!;
    public ICollection<LineItem> LineItems { get; private set; } = new List<LineItem>();
}