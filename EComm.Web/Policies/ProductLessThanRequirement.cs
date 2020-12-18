using EComm.Data.Entities;
using EComm.Data.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EComm.Web.Policies
{
    public class ProductLessThanRequirement : IAuthorizationRequirement
    {
        public decimal Limit;

        public ProductLessThanRequirement(decimal limit)
            => Limit = limit;
    }

    public class ProductLessThanHandler : AuthorizationHandler<ProductLessThanRequirement, Product>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ProductLessThanRequirement requirement, Product product)
        {
            if (product.UnitPrice <= requirement.Limit)
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }
}
