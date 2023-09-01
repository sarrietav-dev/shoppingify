namespace shoppingify.Products.Domain;

public class Product
{
    public required ProductId Id { get; init; }
    public required ProductOwner Owner { get; init; }
    public required string Name { get; init; }
    public required string Category { get; init; }
    public string? Note { get; init; }
    public string? Image { get; init; }
}