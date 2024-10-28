using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using PetProject.Application.AccountManagement;
using PetProject.Infrastructure.Authentification.Options;

namespace PetProject.Infrastructure.Authentification;

public static class Inject
{
    public static IServiceCollection AddFromAuthentificationInfrastructure(
        this IServiceCollection services, IConfiguration config)
    {
        services.Configure<JwtOptions>(
            config.GetSection(JwtOptions.JWT));

        services.AddOptions<JwtOptions>();
        
        services
            .AddIdentity<User, Role>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.Password.RequiredLength = 8;
            })
            .AddEntityFrameworkStores<AuthentificationDbContext>();

        services.AddScoped<AuthentificationDbContext>();
        services.AddTransient<ITokenProvider, JwtTokenProvider>();
        
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
            
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidIssuer = jwtOptions.Issuer,
                ValidAudience = jwtOptions.Audience,
                ValidateLifetime = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Key)),
                ValidateIssuerSigningKey = true,
                ValidateAudience = true,
                ValidateIssuer = true,
                ClockSkew = TimeSpan.Zero
            };
        });

        services.AddAuthentication();

        services.AddAuthorization();

        services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();
        services.AddSingleton<IAuthorizationHandler, PermissionRequirementHandler>();

        return services;
    }
}