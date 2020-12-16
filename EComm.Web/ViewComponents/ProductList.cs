using EComm.Data.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace EComm.Web.ViewComponents
{
    public class ProductList : ViewComponent
    {
        private readonly IRepository _repo;

        public ProductList(IRepository repo)
            => _repo = repo;

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var products = await _repo.GetAllProducts(true);
            return View(products);
        }
    }
}
