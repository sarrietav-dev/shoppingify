using Shoppingify.Cart.Domain;

namespace Shoppingify.Cart.Application.DTOs;

public class CartOwnerDto
{
    public string Id { get; init; }
    public string? ActiveCartId { get; init; }

    public CartOwnerDto(CartOwner cartOwner)
    {
        Id = cartOwner.Id.ToString();
        ActiveCartId = cartOwner.ActiveCart?.ToString();
    }

    public CartOwner ToCartOwner()
    {
        var owner = new CartOwner()
        {
            Id = new CartOwnerId(Id),
            ActiveCart = ActiveCartId is not null ? new CartId(Guid.Parse(ActiveCartId ?? Guid.Empty.ToString())) : null
        };

        return owner;
    }
}