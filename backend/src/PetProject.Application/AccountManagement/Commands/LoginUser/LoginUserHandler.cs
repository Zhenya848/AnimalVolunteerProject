using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using PetProject.Application.Shared.Interfaces;
using PetProject.Domain.Shared;
using PetProject.Infrastructure.Authentification;

namespace PetProject.Application.AccountManagement.Commands.LoginUser;

public class LoginUserHandler : ICommandHandler<LoginUserCommand, Result<string, ErrorList>>
{
    private readonly UserManager<User> _userManager;
    private readonly ILogger<User> _logger;
    private readonly ITokenProvider _tokenProvider;

    public LoginUserHandler(UserManager<User> userManager, ILogger<User> logger, ITokenProvider tokenProvider)
    {
        _userManager = userManager;
        _logger = logger;
        _tokenProvider = tokenProvider;
    }
    
    public async Task<Result<string, ErrorList>> Handle(
        LoginUserCommand userCommand, 
        CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByEmailAsync(userCommand.Email);
        
        if (user == null)
            return (ErrorList)Errors.User.NotFound(userCommand.Email);
        
        var passwordConfirmed = await _userManager.CheckPasswordAsync(user, userCommand.Password);

        if (passwordConfirmed == false)
            return (ErrorList)Errors.User.WrongCredentials();

        var accessToken = _tokenProvider.GenerateAccessToken(user);
        _logger.LogInformation("Login successfully");
        
        return accessToken;
    }
}