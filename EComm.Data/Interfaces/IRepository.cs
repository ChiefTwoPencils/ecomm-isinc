using EComm.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EComm.Data.Interfaces
{
    public interface IRepository
    {
        Task<IEnumerable<Product>> GetAllProducts(bool includeSupplier = false);
        Task<Product> GetProduct(int id, bool includeSupplier = false);
        Task<Product> UpdateProduct(Product product);
        Task<IEnumerable<Supplier>> GetAllSuppliers();
    }
}
