using PetProject.Infrastructure.Authentification;

namespace PetProject.Application.AccountManagement;

public interface ITokenProvider
{
    string GenerateAccessToken(User user);
}