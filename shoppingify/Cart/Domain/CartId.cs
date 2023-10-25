namespace Shoppingify.Cart.Domain;

public record CartId(Guid Value)
{
    public override string ToString()
    {
        return Value.ToString();
    }
};