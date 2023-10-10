using Microsoft.EntityFrameworkCore;
using shoppingify.Cart.Domain;

namespace shoppingify.Cart.Infrastructure.Persistence;

class EFCartOwnerRepository : ICartOwnerRepository
{
    private readonly AppDbContext _context;

    public EFCartOwnerRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task Add(CartOwner cartOwner)
    {
        await _context.AddAsync(cartOwner);
    }

    public async Task<CartOwner?> Get(CartOwnerId id)
    {
        return await _context.CartOwners.Include(x => x.ActiveCart).FirstOrDefaultAsync(x => x.Id == id);
    }
}