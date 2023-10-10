namespace Shoppingify.Products.Application.Commands;

public record AddProductCommand(string Name, string? Note, string Category, string? Image)
{
}