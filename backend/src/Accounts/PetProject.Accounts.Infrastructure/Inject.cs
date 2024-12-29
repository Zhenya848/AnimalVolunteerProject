using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using PetProject.Accounts.Application;
using PetProject.Accounts.Application.Repositories;
using PetProject.Accounts.Domain.User;
using PetProject.Accounts.Presentation;
using PetProject.Accounts.Presentation.Options;
using PetProject.Core;
using PetProject.Core.Application.Abstractions;
using PetProject.Infrastructure.Authentification.Options;
using PetProject.Infrastructure.Authentification.Repositories;
using PetProject.Infrastructure.Authentification.Seeding;

namespace PetProject.Infrastructure.Authentification;

public static class Inject
{
    public static IServiceCollection AddFromAccountsInfrastructure(
        this IServiceCollection services, IConfiguration config)
    {
        services.Configure<JwtOptions>(
            config.GetSection(JwtOptions.JWT));
        
        services.Configure<AdminOptions>(
            config.GetSection(AdminOptions.ADMIN));

        services.Configure<RefreshSessionOptions>(
            config.GetSection(RefreshSessionOptions.RefreshSession));

        services.AddOptions<JwtOptions>();
        services.AddOptions<AdminOptions>();
        services.AddOptions<RefreshSession>();
        
        services
            .AddIdentity<User, Role>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.Password.RequiredLength = 8;
            })
            .AddEntityFrameworkStores<AccountsDbContext>();

        services.AddScoped<AccountsDbContext>();
        
        services.AddSingleton<AccountsSeeder>();
        services.AddScoped<AccountsSeederService>();
        
        services.AddTransient<ITokenProvider, JwtTokenProvider>();
        services.AddSingleton<INotificationProvider, NotificationProvider>();
        
        services.AddKeyedScoped<IUnitOfWork, UnitOfWork>(Modules.Accounts);

        services.AddScoped<IAccountRepository, AccountRepository>();
        
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            var jwtOptions = config.GetSection(JwtOptions.JWT).Get<JwtOptions>()
                ?? throw new ApplicationException("Missing JWT configuration");

            options.TokenValidationParameters = TokenValidationParametersFactory
                .CreateWithLifeTime(jwtOptions);
        });

        services.AddAuthentication();

        services.AddAuthorization();

        return services;
    }
}