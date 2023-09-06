using Microsoft.AspNetCore.Mvc;
using shoppingify.IAM.Application;
using shoppingify.IAM.Application.Exceptions;

namespace shoppingify.IAM.Infrastructure.Controllers;

[ApiController]
[Route("api/v1/user")]
public class IamController : ControllerBase
{
    private readonly IdentityApplicationService _identityApplicationService;

    public IamController(IdentityApplicationService identityApplicationService)
    {
        _identityApplicationService = identityApplicationService;
    }

    [HttpPost]
    public async Task<IActionResult> RegisterUser()
    {
        try
        {
            var uid = Request.Headers["Authorization"].ToString().Split(" ")[1];
            await _identityApplicationService.RegisterUser(uid);

            return CreatedAtAction(nameof(RegisterUser), null);
        }
        catch (InvalidTokenException e)
        {
            return BadRequest(e.Message);
        }
    }
}