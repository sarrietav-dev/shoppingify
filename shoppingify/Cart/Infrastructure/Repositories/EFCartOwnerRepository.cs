using Microsoft.EntityFrameworkCore;
using Shoppingify;
using Shoppingify.Cart.Domain;

namespace Shoppingify.Cart.Infrastructure.Repositories;

internal class EfCartOwnerRepository : ICartOwnerRepository
{
    private readonly AppDbContext _context;

    public EfCartOwnerRepository(AppDbContext context)
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