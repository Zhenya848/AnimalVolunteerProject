using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetProject.Accounts.Application.Commands.CreateUser;
using PetProject.Accounts.Application.Commands.GetInfoAboutUser;
using PetProject.Accounts.Application.Commands.GetUsers;
using PetProject.Accounts.Application.Commands.LoginUser;
using PetProject.Accounts.Application.Commands.RefreshTokens;
using PetProject.Accounts.Application.Commands.SendNotification;
using PetProject.Accounts.Application.Commands.UpdateNotificationSettings;
using PetProject.Accounts.Presentation.Requests;
using PetProject.Framework;
using PetProject.Framework.Authorization;

namespace PetProject.Accounts.Presentation;

public class AccountController : ApplicationController
{
    [HttpPost("registration")]
    public async Task<IActionResult> Register(
        [FromBody] CreateUserRequest request,
        [FromServices] CreateUserHandler handler,
        CancellationToken cancellationToken = default)
    {
        var command = new CreateUserCommand(
            request.Email, 
            request.UserName, 
            request.FullName,
            request.Password, 
            request.SocialNetworks);
        
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

    [HttpPost("refresh")]
    public async Task<IActionResult> RefreshTokens(
        [FromBody] RefreshTokensRequest request,
        [FromServices] RefreshTokensHandler handler,
        CancellationToken cancellationToken = default)
    {
        var command = new RefreshTokensCommand(request.AccessToken, request.RefreshToken);
        var result = await handler.Handle(command, cancellationToken);
        
        if (result.IsFailure)
            return result.Error.ToResponse();
        
        return Ok(result.Value);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetInfoAboutUser(
        [FromRoute] Guid id,
        [FromServices] GetInfoAboutUserHandler handler,
        CancellationToken cancellationToken = default)
    {
        var result = await handler.Handle(id, cancellationToken);
        
        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }

    [HttpPost("send_notification")]
    public async Task<IActionResult> SendNotificationForUsers(
        [FromBody] SendNotificationRequest request,
        [FromServices] SendNotificationHandler handler,
        CancellationToken cancellationToken = default)
    {
        var command = new SendNotificationCommand(request.Users, request.Roles, request.Message);
        var result = await handler.Handle(command, cancellationToken);
        
        if (result.IsFailure)
            return result.Error.ToResponse();
        
        return Ok();
    }
    
    [HttpPost("users")]
    public async Task<IActionResult> GetUsers(
        [FromBody] GetUsersRequest request,
        [FromServices] GetUsersHandler handler,
        CancellationToken cancellationToken = default)
    {
        var command = new GetUsersCommand(request.Users, request.Roles);
        var result = await handler.Handle(command, cancellationToken);

        return Ok(result);
    }

    [HttpPost("notification_settings")]
    public async Task<IActionResult> UpdateNotificationSettings(
        [FromBody] UpdateNotificationSettingsRequest request,
        [FromServices] UpdateNotificationSettingsHandler handler,
        CancellationToken cancellationToken = default)
    {
        var command = new UpdateNotificationSettingsCommand(
            request.UserId, 
            request.Email, 
            request.Telegram,
            request.Web);
        
        var result = await handler.Handle(command, cancellationToken);
        
        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok();
    }
}