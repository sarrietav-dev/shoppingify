using Bogus;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using shoppingify.IAM.Application;
using shoppingify.Products.Application;
using shoppingify.Products.Domain;
using shoppingify.Products.Infrastructure.Controllers;

namespace Shoppingify.Tests.Products;

public class ProductsControllerTest
{
    private readonly Mock<IProductApplicationService> _applicationServiceMock;
    private readonly Mock<IAuthenticationProviderService> _authenticationProviderServiceMock;
    private readonly ProductsController _controller;
    private readonly Faker<Product> _productFaker;

    public ProductsControllerTest()
    {
        _applicationServiceMock = new Mock<IProductApplicationService>();
        _authenticationProviderServiceMock = new Mock<IAuthenticationProviderService>();
        _controller = new ProductsController(_applicationServiceMock.Object, _authenticationProviderServiceMock.Object);
        _productFaker = new Faker<Product>()
            .RuleFor(p => p.Id, f => new ProductId(f.Random.Guid()))
            .RuleFor(p => p.Name, f => f.Random.String())
            .RuleFor(p => p.Category, f => f.Random.String())
            .RuleFor(p => p.Note, f => f.Random.String())
            .RuleFor(p => p.Image, f => f.Random.String())
            .RuleFor(p => p.Owner, f => new ProductOwner(f.Random.String()));
    }

    [Fact]
    public async Task Get_ReturnsNotFound_WhenProductIsNull()
    {
        // Arrange
        var id = Guid.NewGuid();
        _applicationServiceMock.Setup(x => x.Get(id)).ReturnsAsync((Product?)null);

        // Act
        var result = await _controller.Get(id);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task Get_ReturnsOk_WhenProductIsNotNull()
    {
        var product = _productFaker.Generate();
        _applicationServiceMock.Setup(x => x.Get(product.Id.Value)).ReturnsAsync(product);

        // Act
        var result = await _controller.Get(product.Id.Value);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(product, okResult.Value);
    }

    [Fact]
    public async Task GetAll_ReturnsOk_WithListOfProducts()
    {
        // Arrange
        var id = Guid.NewGuid();
        var products = _productFaker.Generate(10);
        _authenticationProviderServiceMock.Setup(x => x.VerifyToken(It.IsAny<string>())).ReturnsAsync(id.ToString());
        _applicationServiceMock.Setup(x => x.GetAll(id.ToString())).ReturnsAsync(products);

        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext()
        };

        _controller.HttpContext.Request.Headers["Authorization"] = "Bearer " + "test";

        // Act
        var result = await _controller.GetAll();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(products, okResult.Value);
    }

    [Fact]
    public async Task Add_ReturnsOk_WhenProductIsAdded()
    {
        // Arrange
        var id = Guid.NewGuid();
        var product = new AddProductCommand("Test Product", "Test Category", "Test Note", "Test Image");
        _authenticationProviderServiceMock.Setup(x => x.VerifyToken(It.IsAny<string>())).ReturnsAsync(id.ToString());

        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext()
        };

        _controller.HttpContext.Request.Headers["Authorization"] = "Bearer " + "test";

        // Act
        var result = await _controller.Add(product);

        // Assert
        Assert.IsType<OkResult>(result);
        _applicationServiceMock.Verify(x => x.Add(id.ToString(), product), Times.Once);
    }

    [Fact]
    public async Task Delete_ReturnsOk_WhenProductIsDeleted()
    {
        // Arrange
        var id = Guid.NewGuid();
        var productId = Guid.NewGuid();

        // Act
        var result = await _controller.Delete(productId);

        // Assert
        Assert.IsType<OkResult>(result);
        _applicationServiceMock.Verify(x => x.Delete(productId), Times.Once);
    }
}