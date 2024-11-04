using Microsoft.Extensions.DependencyInjection;
using PetProject.Accounts.Contracts;

namespace PetProject.Accounts.Implementation;

public static class Inject
{
    public static IServiceCollection AddFromImplementation(
        this IServiceCollection services)
    {
        return services.AddScoped<IAccountsContract, AccountsContract>();
    }
}