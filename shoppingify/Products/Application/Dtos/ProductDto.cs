using Shoppingify.Products.Domain;

namespace Shoppingify.Products.Application.Dtos;

public class ProductDto
{
    public required string Id { get; init; }
    public required string Owner { get; init; }
    public required string Name { get; init; }
    public required string Category { get; init; }
    public string? Note { get; init; }
    public string? Image { get; init; }

    public Product ToProduct()
    {
        return new Product
        {
            Id = new ProductId(Guid.Parse(Id)),
            Owner = new ProductOwner(Owner),
            Name = Name,
            Category = Category,
            Note = Note,
            Image = Image
        };
    }

    public static ProductDto FromProduct(Product product)
    {
        return new ProductDto
        {
            Id = product.Id.Value.ToString(),
            Owner = product.Owner.Value,
            Name = product.Name,
            Category = product.Category,
            Note = product.Note,
            Image = product.Image
        };
    }
}