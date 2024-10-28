using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetProject.Infrastructure.Authentification
{
    public class PermissionAttribute : AuthorizeAttribute, IAuthorizationRequirement
    {
        public string Code;

        public PermissionAttribute(string code) : base(policy: code)
        {
            Code = code;
        }
    }
}
