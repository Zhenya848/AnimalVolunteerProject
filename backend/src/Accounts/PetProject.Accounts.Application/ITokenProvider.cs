using System.Security.Claims;
using CSharpFunctionalExtensions;
using PetProject.Accounts.Application.Models;
using PetProject.Accounts.Domain.User;
using PetProject.Core;

namespace PetProject.Accounts.Application;

public interface ITokenProvider
{
    JwtTokenResult GenerateAccessToken(User user);
    Task<Guid> GenerateRefreshToken(User user, Guid accessTokenJti, CancellationToken cancellationToken = default);
    Task<Result<IReadOnlyList<Claim>, ErrorList>> GetUserClaims(string jwtToken);
}