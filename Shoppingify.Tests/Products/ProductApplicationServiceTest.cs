using Bogus;
using Moq;
using Shoppingify.Products.Application;
using Shoppingify.Products.Application.Commands;
using Shoppingify.Products.Application.Dtos;
using Shoppingify.Products.Domain;

namespace Shoppingify.Tests.Products;

public class ProductApplicationServiceTest
{
    private readonly Mock<IProductRepository> _productRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly IProductApplicationService _service;
    private readonly Faker<Product> _productFaker;

    public ProductApplicationServiceTest()
    {
        _productRepositoryMock = new Mock<IProductRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _service = new ProductApplicationService(_productRepositoryMock.Object, _unitOfWorkMock.Object);
        _productFaker = new Faker<Product>()
            .RuleFor(x => x.Id, f => new ProductId(f.Random.Guid()))
            .RuleFor(x => x.Owner, f => new ProductOwner(f.Random.Guid().ToString()))
            .RuleFor(x => x.Name, f => f.Commerce.ProductName())
            .RuleFor(x => x.Note, f => f.Lorem.Sentence())
            .RuleFor(x => x.Category, f => f.Commerce.Categories(1).First())
            .RuleFor(x => x.Image, f => f.Image.PicsumUrl());
    }

    [Fact]
    public async Task Get_ShouldReturnProduct_WhenProductExists()
    {
        // Arrange
        var product = _productFaker.Generate();

        _productRepositoryMock.Setup(x => x.Get(It.IsAny<ProductId>()))
            .ReturnsAsync(product);

        // Act
        var result = await _service.Get(product.Id.Value);

        // Assert
        Assert.Equal(ProductDto.FromProduct(product), result);
    }

    [Fact]
    public async Task Get_ShouldReturnNull_WhenProductDoesNotExist()
    {
        // Arrange
        _productRepositoryMock.Setup(x => x.Get(It.IsAny<ProductId>()))
            .ReturnsAsync((Product?)null);

        // Act
        var result = await _service.Get(Guid.NewGuid());

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetAll_ShouldReturnProducts_WhenProductsExist()
    {
        // Arrange
        var products = _productFaker.Generate(3);

        _productRepositoryMock.Setup(x => x.GetAll(It.IsAny<ProductOwner>()))
            .ReturnsAsync(products);

        // Act
        var result = await _service.GetAll(products.First().Owner.Value);

        // Assert
        Assert.Equal(products.Count, result.Count());
    }

    [Fact]
    public async Task GetAll_ShouldReturnEmptyList_WhenProductsDoNotExist()
    {
        // Arrange
        _productRepositoryMock.Setup(x => x.GetAll(It.IsAny<ProductOwner>()))
            .ReturnsAsync(new List<Product>());

        // Act
        var result = await _service.GetAll(Guid.NewGuid().ToString());

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public async Task Add_ShouldAddProduct_WhenProductDoesNotExist()
    {
        // Arrange
        var product = _productFaker.Generate();
        var command = new AddProductCommand(Name: product.Name, Note: product.Note, Category: product.Category,
            Image: product.Image);

        _productRepositoryMock.Setup(x => x.Add(It.IsAny<Product>()))
            .Returns(Task.CompletedTask);

        // Act
        await _service.Add(product.Owner.Value, command);

        // Assert
        _productRepositoryMock.Verify(x => x.Add(It.Is<Product>(p =>
            p.Owner == product.Owner &&
            p.Name == product.Name &&
            p.Note == product.Note &&
            p.Category == product.Category &&
            p.Image == product.Image
        )), Times.Once);

        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(default), Times.Once);
    }

    [Fact]
    public async Task Delete_ShouldDeleteProduct_WhenProductExists()
    {
        // Arrange
        var product = _productFaker.Generate();

        _productRepositoryMock.Setup(x => x.Delete(It.IsAny<ProductId>()))
            .Returns(Task.CompletedTask);

        // Act
        await _service.Delete(product.Id.Value);

        // Assert
        _productRepositoryMock.Verify(x => x.Delete(It.Is<ProductId>(p =>
            p == product.Id
        )), Times.Once);

        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(default), Times.Once);
    }
}