namespace shoppingify.Entities;

class Product
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string Note { get; private set; }
    public string Category { get; private set; }
    public string Image { get; private set; }
}