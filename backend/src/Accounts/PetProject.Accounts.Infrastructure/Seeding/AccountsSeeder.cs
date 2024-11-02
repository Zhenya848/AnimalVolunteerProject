using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using PetProject.Infrastructure.Authentification.Options;

namespace PetProject.Infrastructure.Authentification.Seeding;

public class AccountsSeeder(IServiceScopeFactory serviceScopeFactory)
{
    public async Task SeedAsync()
    {
        using var scope = serviceScopeFactory.CreateScope();
        
        var service = scope.ServiceProvider.GetRequiredService<AccountsSeederService>();
        var adminOptions = scope.ServiceProvider.GetRequiredService<IOptions<AdminOptions>>().Value;
        
        await service.SeedAsync(adminOptions);
    }
}