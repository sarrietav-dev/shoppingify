using Shoppingify.Products.Application.Commands;
using Shoppingify.Products.Application.Dtos;
using Shoppingify.Products.Domain;

namespace Shoppingify.Products.Application;

public interface IProductApplicationService
{
    Task<ProductDto?> Get(Guid productId);
    Task<IEnumerable<ProductDto>> GetAll(string productOwnerId);
    Task<ProductDto> Add(string ownerId, AddProductCommand product);
    Task Delete(Guid productId);
}