namespace Shoppingify.Cart.Domain;

public record CartId(Guid Value, DateTime CreatedAt = default);