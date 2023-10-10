using shoppingify.Products.Application.Commands;
using Shoppingify.Products.Domain;

namespace Shoppingify.Products.Application;

public interface IProductApplicationService
{
    Task<Product?> Get(Guid productId);
    Task<IEnumerable<Product>> GetAll(string productOwnerId);
    Task Add(string ownerId, AddProductCommand product);
    Task Delete(Guid productId);
}