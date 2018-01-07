using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using PostOfficeApp.Models;

namespace PostOfficeApp.Authorization
{
    public class AdministratorArthorizationHandler:AuthorizationHandler<OperationAuthorizationRequirement,Newspaper>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, 
            OperationAuthorizationRequirement requirement, Newspaper resource)
        {
            if (context.User == null)
            {
                return Task.FromResult(0);
            }
            if (context.User.IsInRole(Constants.AdministratorRole))
            {
                context.Succeed(requirement);
            }
            return Task.FromResult(0);
        }
    }
}
