using Microsoft.EntityFrameworkCore;
using Shoppingify.Cart.Domain;

namespace Shoppingify.Cart.Infrastructure.Persistence;

internal class EFCartRepository : ICartRepository
{
    public readonly AppDbContext _context;

    public EFCartRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task Add(Domain.Cart cart)
    {
        await _context.AddAsync(cart);
    }

    public async Task<Domain.Cart?> Get(CartId id)
    {
        return await _context.Carts.Include(x => x.CartItems).FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<IEnumerable<Domain.Cart>> GetAll(string ownerId)
    {
        return await _context.Carts.Where(x => x.CartOwnerId.Value == ownerId).ToListAsync();
    }
}