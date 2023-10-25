namespace Shoppingify.Cart.Domain;

public record CartOwnerId(string Value)
{
    public override string ToString()
    {
        return Value;
    }
};