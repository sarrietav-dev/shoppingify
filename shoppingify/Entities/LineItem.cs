namespace shoppingify.Entities;

class LineItem
{
    private Guid _id;
    private Product _product;
    private int _quantity;
    
    public LineItem(Guid id, Product product, int quantity)
    {
        _id = id;
        _product = product;
        _quantity = quantity;
    }
}