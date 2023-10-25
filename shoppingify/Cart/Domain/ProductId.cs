namespace Shoppingify.Cart.Domain;

public record ProductId(Guid Value)
{
    public override string ToString()
    {
        return Value.ToString();
    }
};