using EComm.Data.Interfaces;
using EComm.Web.API.gRPC;
using Grpc.Core;
using System.Threading.Tasks;

namespace EComm.Web.Api.gRPC
{
    public class ECommGrpcService : ECommGrpc.ECommGrpcBase
    {
        private readonly IRepository _repo;

        public ECommGrpcService(IRepository repo)
            => _repo = repo;

        public override async Task<ProductReply> GetProduct(ProductRequest request, ServerCallContext context)
        {
            var product = await _repo.GetProduct(request.Id, true);
            var reply = new ProductReply
            {
                Id = product.Id,
                Name = product.ProductName,
                Price = (double)product.UnitPrice.Value,
                Supplier = product.Supplier.CompanyName
            };
            return reply;
        }
    }
}
