using System.Security.Claims;
using Bogus;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Shoppingify.IAM.Application;
using Shoppingify.Products.Application;
using Shoppingify.Products.Application.Commands;
using Shoppingify.Products.Domain;
using Shoppingify.Products.Infrastructure.Controllers;

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
        _controller = new ProductsController(_applicationServiceMock.Object);
        _productFaker = new Faker<Product>()
            .RuleFor(p => p.Id, f => new ProductId(f.Random.Guid()))
            .RuleFor(p => p.Name, f => f.Random.String())
            .RuleFor(p => p.Category, f => f.Random.String())
            .RuleFor(p => p.Note, f => f.Random.String())
            .RuleFor(p => p.Image, f => f.Random.String())
            .RuleFor(p => p.Owner, f => new ProductOwner(f.Random.String()));

        var user = new ClaimsPrincipal(
            new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString())
            })
        );

        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = user }
        };
    }

    [Fact]
    public async Task Get_WhenProductIsNull_ReturnsNotFound()
    {
        // Arrange
        var id = Guid.NewGuid();
        _applicationServiceMock.Setup(x => x.Get(It.IsAny<Guid>())).ReturnsAsync((Product?)null);

        // Act
        var result = await _controller.Get(id);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }
    
    [Fact]
    public async Task Get_WhenUserIsNotAuthenticated_ReturnsBadRequest()
    {
        // Arrange
        var id = Guid.NewGuid();
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = new ClaimsPrincipal() }
        };

        // Act
        var result = await _controller.Get(id);

        // Assert
        Assert.IsType<BadRequestResult>(result);
    }

    [Fact]
    public async Task Get_WithValidData_ReturnsOk()
    {
        var product = _productFaker.Generate();
        _applicationServiceMock.Setup(x => x.Get(It.IsAny<Guid>())).ReturnsAsync(product);

        // Act
        var result = await _controller.Get(product.Id.Value);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(product, okResult.Value);
    }
    
    [Fact]
    public async Task GetAll_WhenUserIsNotAuthenticated_ReturnsBadRequest()
    {
        // Arrange
        var id = Guid.NewGuid();
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = new ClaimsPrincipal() }
        };

        // Act
        var result = await _controller.GetAll();

        // Assert
        Assert.IsType<BadRequestResult>(result);
    }

    [Fact]
    public async Task GetAll_WithValidData_ReturnsOk()
    {
        // Arrange
        var id = Guid.NewGuid();
        var products = _productFaker.Generate(10);
        _applicationServiceMock.Setup(x => x.GetAll(It.IsAny<string>())).ReturnsAsync(products);

        // Act
        var result = await _controller.GetAll();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(products, okResult.Value);
    }
    
    [Fact]
    public async Task GetAll_WhenNoProducts_ReturnsOk()
    {
        // Arrange
        var id = Guid.NewGuid();
        var products = new List<Product>();
        _applicationServiceMock.Setup(x => x.GetAll(It.IsAny<string>())).ReturnsAsync(products);

        // Act
        var result = await _controller.GetAll();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(products, okResult.Value);
    }

    [Fact]
    public async Task Add_WhenProductIsAdded_ReturnsOk()
    {
        // Arrange
        var id = Guid.NewGuid();
        var product = new AddProductCommand("Test Product", "Test Category", "Test Note", "Test Image");
        _authenticationProviderServiceMock.Setup(x => x.VerifyToken(It.IsAny<string>())).ReturnsAsync(id.ToString());
        _applicationServiceMock.Setup(x => x.Add(It.IsAny<string>(), It.IsAny<AddProductCommand>()))
            .Returns(Task.FromResult(_productFaker.Generate()));

        // Act
        var result = await _controller.Add(product);

        // Assert
        Assert.IsType<CreatedAtActionResult>(result);
        _applicationServiceMock.Verify(x => x.Add(It.IsAny<string>(), product), Times.Once);
    }
    
    [Fact]
    public async Task Add_WhenUserIsNotAuthenticated_ReturnsBadRequest()
    {
        // Arrange
        var id = Guid.NewGuid();
        var product = new AddProductCommand("Test Product", "Test Category", "Test Note", "Test Image");
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = new ClaimsPrincipal() }
        };

        // Act
        var result = await _controller.Add(product);

        // Assert
        Assert.IsType<BadRequestResult>(result);
        _applicationServiceMock.Verify(x => x.Add(It.IsAny<string>(), product), Times.Never);
    }

    [Fact]
    public async Task Delete_WhenProductIsDeleted_ReturnsOk()
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
    
    [Fact]
    public async Task Delete_WhenUserIsNotAuthenticated_ReturnsBadRequest()
    {
        // Arrange
        var id = Guid.NewGuid();
        var productId = Guid.NewGuid();
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = new ClaimsPrincipal() }
        };

        // Act
        var result = await _controller.Delete(productId);

        // Assert
        Assert.IsType<BadRequestResult>(result);
        _applicationServiceMock.Verify(x => x.Delete(productId), Times.Never);
    }
}