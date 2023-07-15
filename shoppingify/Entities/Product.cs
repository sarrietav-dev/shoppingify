namespace shoppingify.Entities;

public class Product
{
    private Guid Id { get; } = Guid.NewGuid();
    public required string Name { get; init; }
    public required string Note { get; init; }
    public required string Category { get; init; }
    public required string Image { get; init; }

    public override bool Equals(object? obj)
    {
        return obj is Product product &&
               EqualityComparer<Guid>.Default.Equals(Id, product.Id);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Id);
    }
}