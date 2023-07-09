namespace shoppingify.Entities;

class LineItem
{
    public Guid Id { get; private set; }
    public Product Product { get; private set; } = null!;
    public int Quantity { get; private set; }
}