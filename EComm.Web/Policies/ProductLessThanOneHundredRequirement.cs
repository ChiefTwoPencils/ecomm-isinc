using EComm.Data.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Threading.Tasks;

namespace EComm.Web.Policies
{
    public class ProductLessThanOneHundredRequirement : IAuthorizationRequirement
    { }

    public class ProductLessThanOneHundredHandler : AuthorizationHandler<ProductLessThanOneHundredRequirement>
    {
        private readonly IRepository _repo;

        public ProductLessThanOneHundredHandler(IRepository repo)
            => _repo = repo;

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, ProductLessThanOneHundredRequirement requirement)
        {
            var random = new Random();
            var product = await _repo.GetProduct(random.Next(1, 42));
            if (product.UnitPrice <= 100)
            {
                context.Succeed(requirement);
            } 
        }
    }
}
