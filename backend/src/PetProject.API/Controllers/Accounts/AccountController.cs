using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetProject.API.Controllers.Accounts.Requests;
using PetProject.Application.AccountManagement.Commands;
using PetProject.Application.AccountManagement.Commands.LoginUser;
using PetProject.Infrastructure.Authentification;

namespace PetProject.API.Controllers.Accounts;

public class AccountController : ApplicationController
{
    [HttpPost("registration")]
    public async Task<IActionResult> Register(
        [FromBody] CreateUserRequest request,
        [FromServices] CreateUserHandler handler,
        CancellationToken cancellationToken = default)
    {
        var command = new CreateUserCommand(request.Email, request.UserName, request.Password);
        var result = await handler.Handle(command, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Created();
    }
    
    [HttpPost("login")]
    public async Task<IActionResult> Login(
        [FromBody] LoginUserRequest request,
        [FromServices] LoginUserHandler handler,
        CancellationToken cancellationToken = default)
    {
        var command = new LoginUserCommand(request.Email, request.Password);
        var result = await handler.Handle(command, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return new ObjectResult(result.Value) { StatusCode = StatusCodes.Status201Created };
    }

    [Permission("Admin")]
    [HttpGet("admin")]
    public async Task<IActionResult> Admin()
    {
        return Ok("Admin");
    }
}