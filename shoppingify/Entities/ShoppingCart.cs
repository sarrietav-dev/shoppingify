namespace shoppingify.Entities;

class ShoppingCart
{
    private Guid _id;
    private string _name;
    private ICollection<LineItem> _lineItems;
    
    public ShoppingCart(Guid id, string name, ICollection<LineItem> lineItems)
    {
        _id = id;
        _name = name;
        _lineItems = lineItems;
    }
}