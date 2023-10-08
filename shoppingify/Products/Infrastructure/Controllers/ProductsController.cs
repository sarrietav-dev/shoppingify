using Microsoft.AspNetCore.Mvc;
using shoppingify.IAM.Application;
using shoppingify.Products.Application;

namespace shoppingify.Products.Infrastructure.Controllers;

[ApiController]
[Route("api/v1/me/products")]
public class ProductsController : ControllerBase
{
    private readonly IProductApplicationService _applicationService;
    private readonly IAuthenticationProviderService _authenticationProviderService;

    public ProductsController(IProductApplicationService applicationService, IAuthenticationProviderService authenticationProviderService)
    {
        _applicationService = applicationService;
        _authenticationProviderService = authenticationProviderService;
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Get(Guid id)
    {
        var product = await _applicationService.Get(id);

        if (product is null)
            return NotFound();

        return Ok(product);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var id = await GetAuthorizationToken();
        
        var products = await _applicationService.GetAll(id);

        return Ok(products);
    }

    [HttpPost]
    public async Task<IActionResult> Add(AddProductCommand product)
    {
        var id = await GetAuthorizationToken();
        
        await _applicationService.Add(ownerId: id, product);

        return Ok();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _applicationService.Delete(id);

        return Ok();
    }
    
    private async Task<string> GetAuthorizationToken()
    {
        var uid = Request.Headers["Authorization"].ToString().Split(" ")[1];
        var cartOwnerId = await _authenticationProviderService.VerifyToken(uid);
        return cartOwnerId;
    }
}