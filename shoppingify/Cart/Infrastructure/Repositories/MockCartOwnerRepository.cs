using shoppingify.Cart.Domain;

namespace shoppingify.Cart.Infrastructure.Repositories;

public class MockCartOwnerRepository : ICartOwnerRepository
{
    private readonly HashSet<CartOwner> _cartOwners = new();

    public IReadOnlySet<CartOwner> CartOwners
    {
        get => _cartOwners;
        init => _cartOwners = value.ToHashSet();
    }

    public Task<CartOwner?> Get(CartOwnerId id)
    {
        return Task.FromResult(_cartOwners.SingleOrDefault(x => x.Id == id));
    }

    public Task Add(CartOwner cartOwner)
    {
        _cartOwners.Add(cartOwner);
        return Task.CompletedTask;
    }
}