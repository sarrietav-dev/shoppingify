namespace shoppingify.Entities;

public class Product
{
    public Guid Id { get; } = Guid.NewGuid();
    public required string Name { get; init; }
    public required string Note { get; init; }
    public required string Category { get; init; }
    public required string Image { get; init; }
}