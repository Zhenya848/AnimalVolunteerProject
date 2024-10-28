using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetProject.Infrastructure.Authentification
{
    public class PermissionRequirementHandler : AuthorizationHandler<PermissionAttribute>
    {
        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context, 
            PermissionAttribute requirement)
        {
            var permissions = context.User.Claims.FirstOrDefault(c => c.Type == "user.admin");

            if (permissions == null)
                return;

            if (permissions.Value == requirement.Code)
                context.Succeed(requirement);
        }
    }
}
