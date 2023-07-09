namespace shoppingify.Entities;

class Product
{
    private Guid _id;
    private string _name;
    private string _note;
    private string _category;

    public Product(Guid id, string name, string note, string category)
    {
        _id = id;
        _name = name;
        _note = note;
        _category = category;
    }
}