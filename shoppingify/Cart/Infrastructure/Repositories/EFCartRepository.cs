using Microsoft.EntityFrameworkCore;
using Shoppingify.Cart.Domain;

namespace Shoppingify.Cart.Infrastructure.Repositories;

internal class EfCartRepository : ICartRepository
{
    private readonly AppDbContext _context;

    public EfCartRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task Add(Shoppingify.Cart.Domain.Cart cart)
    {
        await _context.AddAsync(cart);
    }

    public async Task<Shoppingify.Cart.Domain.Cart?> Get(CartId id)
    {
        return await _context.Carts.Include(x => x.CartItems).FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<IEnumerable<Shoppingify.Cart.Domain.Cart>> GetAll(string ownerId)
    {
        var cartOwnerId = new CartOwnerId(ownerId);
        return await _context.Carts.Where(x => x.CartOwnerId == cartOwnerId).ToListAsync();
    }
}