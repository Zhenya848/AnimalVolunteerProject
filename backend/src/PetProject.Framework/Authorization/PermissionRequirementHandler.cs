using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using PetProject.Accounts.Contracts;
using PetProject.Core;

namespace PetProject.Framework.Authorization
{
    public class PermissionRequirementHandler(IServiceScopeFactory serviceScopeFactory) 
        : AuthorizationHandler<PermissionAttribute>
    {
        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context, 
            PermissionAttribute permission)
        {
            var userIdString = context.User.Claims
                .FirstOrDefault(c => c.Type == CustomClaims.Sub)
                ?.Value;
            Guid userId = Guid.Empty;
            
            if (userIdString is null || Guid.TryParse(userIdString, out userId) == false)
                return;
            
            var scope = serviceScopeFactory.CreateScope();
            var accountContract = scope.ServiceProvider.GetRequiredService<IAccountsContract>();

            var permissions = await accountContract.GetUserPermissionCodes(userId);
            
            if (permissions.Contains(permission.Code))
                 context.Succeed(permission);
        }
    }
}
