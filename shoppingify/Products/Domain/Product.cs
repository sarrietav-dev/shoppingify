namespace Shoppingify.Products.Domain;

public class Product
{

    public required ProductId Id { get; init; }
    public required ProductOwner Owner { get; init; }
    public required string Name { get; init; }
    public required string Category { get; init; }
    public string? Note { get; init; }
    public string? Image { get; init; }
    
    public override bool Equals(object? obj)
    {
        if (obj == null || GetType() != obj.GetType())
            return false;

        var product = (Product) obj;
        return Id.Equals(product.Id) && Owner.Equals(product.Owner);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Id, Owner);
    }
    
}