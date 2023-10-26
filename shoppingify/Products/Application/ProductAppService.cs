using Shoppingify.Products.Application.Commands;
using Shoppingify.Products.Application.Dtos;
using Shoppingify.Products.Domain;

namespace Shoppingify.Products.Application;

public class ProductApplicationService : IProductApplicationService
{
    private readonly IProductRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public ProductApplicationService(IProductRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ProductDto?> Get(Guid productId)
    {
        var product = await _repository.Get(new ProductId(productId));

        return product is null ? null : ProductDto.FromProduct(product);
    }

    public async Task<IEnumerable<ProductDto>> GetAll(string productOwnerId)
    {
        var product = await _repository.GetAll(new ProductOwner(productOwnerId));
        
        return product.Select(ProductDto.FromProduct);
    }

    public async Task<ProductDto> Add(string ownerId, AddProductCommand product)
    {
        var newProduct = new Product
        {
            Id = new ProductId(Guid.NewGuid()),
            Owner = new ProductOwner(ownerId),
            Name = product.Name,
            Note = product.Note,
            Category = product.Category,
            Image = product.Image
        };

        await _repository.Add(newProduct);
        await _unitOfWork.SaveChangesAsync();

        return ProductDto.FromProduct(newProduct);
    }

    public async Task Delete(Guid productId)
    {
        await _repository.Delete(new ProductId(productId));
        await _unitOfWork.SaveChangesAsync();
    }
}